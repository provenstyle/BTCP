new function() {

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "Edit$Entity$Controller"
    });

    eval(this.imports);

    const Edit$Entity$Controller = Controller.extend({
        $properties: {
            title:      "Edit $Entity$",
            buttonText: "Save $Entity$",
            isEdit:     true,
            $entityLowercase$: { validate: $nested }
        },

        showEdit$Entity$(data) {
            return $Entity$Feature(this.io).$entityLowercase$(data.id).then($entityLowercase$ => {
                this.$entityLowercase$ = $entityLowercase$;
                return ViewRegion(this.io).show("app/$entityPluralLowercase$/createEdit$Entity$");
            });
        },
        save() {
            return $Entity$Feature(this.ifValid)
                .update$Entity$(this.$entityLowercase$)
                .then($entityLowercase$ => bt.$entityLowercase$.$Entity$Controller(this.io).next(
                    ctrl => ctrl.show$Entity$({id: this.$entityLowercase$.id })));
        },
        cancel() {
            return bt.$entityLowercase$.$Entity$Controller(this.io).next(
                ctrl => ctrl.show$Entity$({ id: this.$entityLowercase$.id }));
        },
        remove() {
            return $Entity$Feature(this.io
                .$confirm(`Delete $Entity$ "${this.$entityLowercase$.name}"?`))
                .remove$Entity$(this.$entityLowercase$).then(() =>
                    bt.$entityLowercase$.$EntityPlural$Controller(this.io).next(ctrl => ctrl.show$EntityPlural$()));
        }
    });

    eval(this.exports);

};
