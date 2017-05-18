new function() {

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "bt,miruken.mvc",
        exports: "$Entity$Controller"
    });

    eval(this.imports);

    const $Entity$Controller = Controller.extend({
        $properties: {
            $entityLowercase$: []
        },

        show$Entity$(data) {
            return $Entity$Feature(this.io).$entityLowercase$(data.id).then($entityLowercase$ => {
                this.$entityLowercase$ = $entityLowercase$;
                return ViewRegion(this.io).show("app/$entityPluralLowercase$/$entityLowercase$");
            });
        },
        goToEdit() {
            return bt.$entityLowercase$.Edit$Entity$Controller(this.io)
                .next(ctrl => ctrl.showEdit$Entity$(this.$entityLowercase$));
        }
    });

    eval(this.exports);

};
