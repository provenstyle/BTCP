new function () {

    base2.package(this, {
        name:     "em",
        imports:  "miruken.ioc,miruken.ng,miruken.mvc,miruken.error,serviceBus,infrastructure",
        exports:  "Configuration,SetupRunner,SetupInstaller",
        ngModule: ["ui.router",
                   "ngMessages",
                   "kendo.directives",
                   "localytics.directives",
                   "ngLocalize",
                   "ngLocalize.InstalledLanguages"]
    });

    eval(this.imports);

    const Configuration = Model.extend({
        $properties: {
            applicationName: undefined,
            clientIp:        undefined,
            dataApiBaseUrl:  undefined,
            locale:          "en-US",
            version:         undefined
        }
    });

    const configuration = new Configuration(appConfig);
    miruken.callback.$provide($appContext, configuration);

    const defaultRoute = new RoutePolicy({
        baseUrl:         configuration.dataApiBaseUrl,
        applicationName: configuration.applicationName
    });
    miruken.callback.$handle($appContext, RoutePolicy, route => defaultRoute.mergeInto(route));

    this.package.ngModule
        .value("localeSupported", ["en-US"])
        .value("localeConf", {
            basePath:         "languages",
            defaultLocale:    configuration.locale,
            sharedDictionary: "common",
            fileExtension:    ".lang.json",
            persistSelection: true,
            cookieName:       "COOKIE_LOCALE_LANG",
            observableAttrs:  new RegExp("^data-(?!ng-|i18n)"),
            delimiter:        "::",
            validTokens:      new RegExp(".*")
        });

    moment.locale(configuration.locale);

    const SetupRunner = Runner.extend({
        $inject: ["$rootScope", "$state", "locale", "$appContext", "$http"],
        constructor($rootScope, $state, locale, $appContext, $http) {
            // Requiring $state in a run forces the  ui-router to
            // register a listener for '$locationChangeSuccess'
            // before it is raised by the  angular.bootstrap function.
            // Also addin it to the $rootScope for convenience
            $rootScope.$state = $state;

            // Capture all ui-router errors and propogate them through
            // the Errors feature for any custom handling
            $rootScope.$on("$stateChangeError",
                (event, toState, toParams, fromState, fromParams, error) => {
                    console.log(`$stateChangeError: ${error}`);

                    //prevent the page from reverting to the previous url and causing an infitate loop on startup
                    event.preventDefault();

                    //wait to make sure the ErrorController is loaded for error occuring on startup
                    setTimeout(() => {
                        const context = event.targetScope.context;
                        return Errors((context || $rootContext).$ngApplyAsync(20))
                            .handleError(error, "ui-router");
                    }, 100);
                });

            $appContext.addHandlers(new ServiceBusHandler($http));
            $appContext.addHandlers(new SideEffectsHandler());
        }
    });

    const SetupInstaller = Installer.extend({
        $inject: ["$stateProvider", "$urlRouterProvider"],
        constructor($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state("em", {
                    abstract: true,
                    template: "<ui-view/>"
                });

            $urlRouterProvider.otherwise('/');
        }
    });

    // Defines the pipeline for Controller operations
    Controller.implement({
        get controllerContext() {
            const context = this.context;
            return context && context
                .$recover() // handle common erros scenarios
                .$validAsync(this)
                .$guard()
                .$ngApplyAsync(20); // ensure $digest loop runs after promise
        },

        initialize() {
            this.base();
            const scope = this.context.resolve("$scope");
            if (scope) {
                const locale = em.Locale(this.context);
                scope.l = locale.localize.bind(locale);
            }
        },

        viewLoaded() {
            setTimeout(function () {
                $("[js-focus]").focus();
            }, 200);
        },

        activity(promise, ms, property) {
            if (miruken.$isPromise(promise)) {
                let enabled = false;
                const scope = this.context.resolve("$scope");
                property = property || "$$activity";
                setTimeout(() => {
                    if (enabled != null) {
                        enabled = true;
                        let activity = this[property] || 0;
                        this[property] = ++activity;
                        scope.$evalAsync();
                    }
                }, ms != null ? ms : 50);
                promise.finally(() => {
                    if (enabled) {
                        let activity = this[property];
                        if (!activity || activity === 1) {
                            delete this[property];
                            if (property in this) {
                                this[property] = undefined;
                            }
                        } else {
                            this[property] = --activity;
                        }
                    }
                    scope.$evalAsync();
                    enabled = undefined;
                });
            }
            return promise;
        },
        replaceState(state, params) {
            const $state = this.context.resolve("$state");
            if ($state) {
                return $state.go(state, params);
            }
        },
        reloadState(state, params) {
            const $state = this.context.resolve("$state");
            if ($state) {
                return $state.go(state, params, { reload: true });
            }
        },
        adoptState(state, params) {
            const $state = this.context.resolve("$state");
            if ($state) {
                return $state.go(state, params, { notify: false });
            }
        }
    });

    eval(this.exports);

};
