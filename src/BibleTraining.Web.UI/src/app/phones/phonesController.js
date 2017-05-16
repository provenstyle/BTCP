new function() {

    bt.package(this, {
        name:    "phone",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "PhonesController"
    });

    eval(this.imports);

    const PhonesController = TableController.extend({
        configure(table) {
            table
                .url("api/phones")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.phone.PhoneController(this.context).next(ctrl => ctrl.showPhone(data));
                });
        },
        showPhones() {
            return ViewRegion(this.io).show("app/phones/phones");
        },
        goToCreate() {
            bt.phone.CreatePhoneController(this.io).next(ctrl => ctrl.showCreatePhone());
        },
        rowSelected(data) {
           bt.phone.PhoneController(this.context).next(ctrl => ctrl.showPhone(data));
        }
    });

    eval(this.exports);

};
