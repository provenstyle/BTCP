new function() {

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "$EntityPlural$Controller"
    });

    eval(this.imports);

    const $EntityPlural$Controller = TableController.extend({
        configure(table) {
            table
                .url("api/$entityPluralLowercase$")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.$entityLowercase$.$Entity$Controller(this.context).next(ctrl => ctrl.show$Entity$(data));
                });
        },
        show$EntityPlural$() {
            return ViewRegion(this.io).show("app/$entityPluralLowercase$/$entityPluralLowercase$");
        },
        goToCreate() {
            bt.$entityLowercase$.Create$Entity$Controller(this.io).next(ctrl => ctrl.showCreate$Entity$());
        },
        rowSelected(data) {
           bt.$entityLowercase$.$Entity$Controller(this.context).next(ctrl => ctrl.show$Entity$(data));
        }
    });

    eval(this.exports);

};
