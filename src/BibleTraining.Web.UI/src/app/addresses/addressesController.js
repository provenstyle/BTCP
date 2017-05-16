new function() {

    bt.package(this, {
        name:    "address",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "AddressesController"
    });

    eval(this.imports);

    const AddressesController = TableController.extend({
        configure(table) {
            table
                .url("api/addresses")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.address.AddressController(this.context).next(ctrl => ctrl.showAddress(data));
                });
        },
        showAddresses() {
            return ViewRegion(this.io).show("app/addresses/addresses");
        },
        goToCreate() {
            bt.address.CreateAddressController(this.io).next(ctrl => ctrl.showCreateAddress());
        },
        rowSelected(data) {
           bt.address.AddressController(this.context).next(ctrl => ctrl.showAddress(data));
        }
    });

    eval(this.exports);

};
