new function(){

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "$Entity$Handler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const $Entity$Result = Result.extend({
        "$type": "BibleTraining.Api.$Entity$.$Entity$Result, BibleTraining.Api",
        $properties: {
            $entityPluralLowercase$: { map: $Entity$ }
        }
    });

    const Get$EntityPlural$ = Request($Entity$Result).extend({
        $properties: {
            "$type": "BibleTraining.Api.$Entity$.Get$EntityPlural$, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const Create$Entity$ = Request($Entity$).extend({
        $properties: {
            "$type": "BibleTraining.Api.$Entity$.Create$Entity$, BibleTraining.Api",
            resource: undefined
        }
    });

    const Update$Entity$ = Request($Entity$).extend({
        $properties: {
            "$type": "BibleTraining.Api.$Entity$.Update$Entity$, BibleTraining.Api",
            resource: undefined
        }
    });

    const Remove$Entity$ = Request($Entity$).extend({
        $properties: {
            "$type": "BibleTraining.Api.$Entity$.Remove$Entity$, BibleTraining.Api",
            resource: undefined
        }
    });

    const $Entity$Handler = CallbackHandler.extend($Entity$Feature, {
        $entityPluralLowercase$() {
            return ServiceBus($composer).process(new Get$EntityPlural$()).then(data => {
                return data.$entityPluralLowercase$;
            });
        },
        $entityLowercase$(id) {
            return ServiceBus($composer).process(new Get$EntityPlural$({ids: [id]})).then(data => {
                return (data.$entityPluralLowercase$ && data.$entityPluralLowercase$.length > 0)
                    ? data.$entityPluralLowercase$[0]
                    : undefined;
            });
        },
        create$Entity$($entityLowercase$) {
            const config = $composer.resolve(Configuration);
            $entityLowercase$.createdBy = $entityLowercase$.modifiedBy = config.userName;
            return ServiceBus($composer).process(new Create$Entity$({ resource: $entityLowercase$ })).then(data => {
                return $entityLowercase$.fromData(data);
            });
        },
        update$Entity$($entityLowercase$) {
            const config = $composer.resolve(Configuration);
            $entityLowercase$.modifiedBy = config.userName;
            return ServiceBus($composer).process(new Update$Entity$({ resource: $entityLowercase$ })).then(data => {
                return $entityLowercase$.fromData(data);
            });
        },
        remove$Entity$($entityLowercase$) {
            const config = $composer.resolve(Configuration);
            $entityLowercase$.modifiedBy = config.userName;
            return ServiceBus($composer).process(new Remove$Entity$({ resource: $entityLowercase$ })).then(data => {
                return $entityLowercase$.fromData(data);
            });
        }
    });

    eval(this.exports);

};
