new function() {

    base2.package(this, {
        name:    "em",
        imports: "miruken,miruken.callback,miruken.context",
        exports: "Localize,Locale,LocaleHandler,LocaleBundleHandler"
    });

    eval(this.imports);

    const Localize = Base.extend({
        $properties: {
            format: undefined,
            args:   undefined,
            result: undefined
        },

        constructor(format, ...args) {
            this.format = format;
            this.args   = args;
        }
    }, {
        coerce() {
            return this.new.apply(this, arguments);
        }
    });

    const Locale = StrictProtocol.extend({
        localize(source, context) {}
    });

    const LocaleHandler = CompositeCallbackHandler.extend(Locale, {
         localize(source, context) {
            if (source) {
                return $isString(source)
                     ? this.getLocalizedString(source) || $NOT_HANDLED
                     : (this.handle(source) ? source.result : $NOT_HANDLED);
            }
        },
        getLocalizedString(string) { return string; },

        $handle: [
            Localize, function (localize) {
                let f = localize.format;
                if (f) {
                    f = this.getLocalizedString(f);
                    if (f) {
                        const args = localize.args;
                        localize.result = args ? format(f, ...args) : f;
                        return;
                    }
                    return $NOT_HANDLED;
                }
            }
        ]
    });

    const LocaleBundleHandler = LocaleHandler.extend({
        constructor(bundle) {
            this.base();
            this.extend({
                getLocalizedString(string) {
                    return bundle[string];
                }
            });
        }
    });

    Context.implement({
        localize(bundles) {
            const root = new LocaleHandler();
            bundles.reduce((parent, bundle) => {
                const handler = new LocaleBundleHandler(bundle);
                parent.addHandlers(handler);
                return handler;
            }, root);
            return this.addHandlers(root);
        }
    });

    eval(this.exports);

};
