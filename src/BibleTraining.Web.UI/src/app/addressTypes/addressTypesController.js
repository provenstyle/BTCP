new function() {

    bt.package(this, {
        name:    "addressType",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "AddressTypesController"
    });

    eval(this.imports);

    const AddressTypesController = TableController.extend({
        configure(table) {
            table
                .url("api/addressTypes")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.addressType.AddressTypeController(this.context).next(ctrl => ctrl.showAddressType(data));
                });
        },
        showAddressTypes() {
            return ViewRegion(this.io).show("app/addressTypes/addressTypes");
        },
        goToCreate() {
            bt.addressType.CreateAddressTypeController(this.io).next(ctrl => ctrl.showCreateAddressType());
        },
        rowSelected(data) {
           bt.addressType.AddressTypeController(this.context).next(ctrl => ctrl.showAddressType(data));
        }
    });

    eval(this.exports);

};
