new function() {

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "Create$Entity$Controller"
    });

    eval(this.imports);

    const Create$Entity$Controller = Controller.extend({
        $properties:{
            title:      "Create $Entity$",
            buttonText: "Create $Entity$",
            isCreate:   true,
            $entityLowercase$: { validate: $nested }
        },
        constructor() {
            this.$entityLowercase$ = new $Entity$();
        },

        showCreate$Entity$() {
            return ViewRegion(this.io).show("app/$entityPluralLowercase$/createEdit$Entity$");
        },
        save() {
            return $Entity$Feature(this.ifValid)
                .create$Entity$(this.$entityLowercase$)
                .then($entityLowercase$ => bt.$entityLowercase$.$Entity$Controller(this.io).next(
                    ctrl => ctrl.show$Entity$({id: this.$entityLowercase$.id })));
        },
        cancel() {
            return bt.$entityLowercase$.$EntityPlural$Controller(this.io).next(
                ctrl => ctrl.show$EntityPlural$());
        }
    });

    eval(this.exports);

};