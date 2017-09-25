new function() {

    bt.package(this, {
        name:    "phoneType",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "PhoneTypesController"
    });

    eval(this.imports);

    const PhoneTypesController = TableController.extend({
        configure(table) {
            table
                .url("api/phoneTypes")
                .text("name", "Name")
                .rowSelected(data => {
                    bt.phoneType.PhoneTypeController(this.context).next(ctrl => ctrl.showPhoneType(data));
                });
        },
        showPhoneTypes() {
            return ViewRegion(this.io).show("app/phoneTypes/phoneTypes");
        },
        goToCreate() {
            bt.phoneType.CreatePhoneTypeController(this.io).next(ctrl => ctrl.showCreatePhoneType());
        },
        rowSelected(data) {
           bt.phoneType.PhoneTypeController(this.context).next(ctrl => ctrl.showPhoneType(data));
        }
    });

    eval(this.exports);

};
