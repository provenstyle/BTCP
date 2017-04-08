new function() {

    base2.package(this, {
        name:    "serviceBus",
        imports: "miruken,miruken.callback,miruken.mvc,miruken.validate",
        exports: "Key,KeysResult,Request,AnonymousRequest,Cached,Concurrent," +
                 "ServiceBus,ServiceBusHandler,dynamic,mapKey,RoutePolicy"
    });

    eval(this.imports);

    const dynamic = Object.freeze({ dynamic: true });

    const Key = Model.extend({
        $properties: {
            id:      undefined,
            orderBy: undefined,
            name:    undefined
        }
    });
    Key.prototype.valueOf = function () { return this.id; };

    const mapKey = Object.freeze({ map: Key });

    const KeysResult = Base.extend({
        $properties: {
            $type: "Improving.MediatR.KeysResult`1[[System.Int32, mscorlib]], Improving.MediatR",
            keys: mapKey
        }
    });

    const Request = Model.extend({
        $properties: {
            $type: undefined   // Ensures $type comes first (Needed by JSON.NET)
        },
        constructor(data) {
            this.base(data, dynamic);
        },

        getPath() {
            let path = "process",
                type = this.$type;
            if (type) {
                const index = type.lastIndexOf(",");
                type = type.substring(0, index)
                    .replace(/[.]?\b[A-Z]/g, function (f) {
                         return f.replace('.', '/').toLowerCase();
                    });
                path = `${path}/${type}`;
            }
            return path;
        },
        mapResponse(response) {
            const responseType = this.responseType;
            if (responseType) {
                return new responseType(response, dynamic);
            }
            return response;
        },
        mapIds(items, mapper) {
            return new Map(Model.map(items, idEntry(mapper), dynamic));
        }
    }, {
        coerce: function(response) {
            if (this === Request) {
                const request = Request.extend();
                if (response) {
                    request.implement({
                        $properties: {
                            responseType: { ignore: true }
                        },
                        get responseType() {
                            return response;
                        }
                    });
                }
                return request;
            }
            return this.new.apply(this, arguments);
        }
    });

    const AnonymousRequest = Request.extend();

    const Cached = Request.extend({
        $properties: {
            $type: "Improving.MediatR.Cache.Cached`1[[%1]], Improving.MediatR",
            request:    undefined,
            timeToLive: "1:00:00"
        },

        constructor(request, timeToLive) {
            const responseType = request.responseType;
            if (!responseType || !responseType.prototype.$type) {
                throw new TypeError("Cached requests must provide have a responseType.$type");
            }
            this.request    = request;
            this.$type      = format(this.$type, responseType.prototype.$type);
            this.timeToLive = timeToLive || this.timeToLive;
        },

        getPath() {
            return `${this.request.getPath()}/cached}`;
        },
        mapResponse(response) {
            return this.request.mapResponse(response);
        }
    });

    Request.implement({
        cached(timeToLive) {
            return new Cached(this, timeToLive || "1:00:00");
        }
    });

    const Concurrent = Request.extend({
        $properties: {
            $type: "Improving.MediatR.Concurrency.Concurrent,Improving.MediatR",
            requests: undefined,
            tag: undefined
        },
        constructor(requests, tag) {
            this.requests = requests;
            this.tag      = tag;
        },

        getPath(applicationName) {
            return this.tag ? `tag/${applicationName}/${this.tag}` : "process";
        },
        mapResponse(response) {
            if (response && response.responses) {
                const responses = [];
                for (let index = 0; index < response.responses.length; ++index) {
                    const resp = response.responses[index];
                    responses.push(this.requests[index].mapResponse(resp));
                }
                return responses;
            }
            return this.base(response);
        }
    });

    let ServiceBusBatch = null;

    const RoutePolicy = Model.extend({
        $properties: {
	    baseUrl:         undefined,
	    applicationName: undefined,
	    timeout:         undefined
	}
    });

    CallbackHandler.implement({
	route(policy) {
	    return policy ? this.decorate({
		$handle: [RoutePolicy, route =>
			  new RoutePolicy(policy).mergeInto(route)]
	    }) : this;
	}
    });

    const ServiceBus = StrictProtocol.extend(Resolving, {
        process(request) {}
    });

    const ServiceBusHandler = Base.extend(ServiceBus, {
        $inject: ["$http"],
        constructor($http) {
            this.extend({
                process(request) {
		    const route    = new RoutePolicy(),
		          hasRoute = $composer.handle(route, true),
			  baseUrl  = route.baseUrl ? route.baseUrl + "/" : "",
			  path     = request.getPath(route.applicationName),
			  config   = route.hasOwnProperty("timeout")
			           ? { timeout: route.timeout }
			           : null;

                    if (!(request instanceof Request)) {
                        request = new AnonymousRequest(request);
                    }
                    const batcher = $composer.getBatcher(ServiceBus);
                    if (batcher) {
                        const batch = new ServiceBusBatch(batcher.tag);
                        batcher.addHandlers(batch);
                        return batch.process(request);
                    }
                    const validation = Validator($composer).validate(request);
                    if (!validation.valid) {
                        return Promise.reject(validation);
                    }
                    return $http.post(`${baseUrl}${path}`, { payload: request }, config)
                        .then(response => {
                            const data = response.data;
                            return data && request.mapResponse(data.payload);
                        });
                }
            });
        }
    });

    ServiceBusBatch = Base.extend(ServiceBus, Batching, {
        constructor(tag) {
            const _groups = new Map();
            this.extend({
                process(request) {
		    const route    = new RoutePolicy(),
		          hasRoute = $composer.handle(route, true),
			  baseUrl  = route.baseUrl;

                    let group = _groups.get(baseUrl);
                    if (!group) {
                        _groups.set(baseUrl, group = {
                            requests:  [],
                            responses: [],
                            promises:  []
                        });
                    }
                    group.requests.push(request);
                    let rejectPromise = Undefined;
                    const promise = new Promise((resolve, reject) => {
                        group.responses.push(resolve);
                        rejectPromise = reject;
                    });
                    extend(promise, "reject", rejectPromise);
                    group.promises.push(promise);
                    return promise;
                },
                complete(composer) {
                    const bus = ServiceBus(composer);
                    return Promise.all([..._groups].map(([url, group]) =>
                        group.requests.length === 1
                            ? bus.process(group.requests[0]).then(response => {
                                group.responses[0](response);
                                return [url, [response]];
                              })
                            : bus.process(Concurrent(group.requests, tag)).then(responses => {
                                for (let i = 0; i < responses.length; ++i) {
                                    group.responses[i](responses[i]);
                                }
                                return Promise.all(group.promises).then(() => [url, responses]);
                              })
                        ));
                }
            });
        }
    });

    eval(this.exports);

};
