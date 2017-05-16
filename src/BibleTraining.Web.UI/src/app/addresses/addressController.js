new function() {

    bt.package(this, {
        name:    "address",
        imports: "bt,miruken.mvc",
        exports: "AddressController"
    });

    eval(this.imports);

    const AddressController = Controller.extend({
        $properties: {
            address: []
        },

        showAddress(data) {
            return AddressFeature(this.io).address(data.id).then(address => {
                this.address = address;
                return ViewRegion(this.io).show("app/addresses/address");
            });
        },
        goToEdit() {
            return bt.address.EditAddressController(this.io)
                .next(ctrl => ctrl.showEditAddress(this.address));
        }
    });

    eval(this.exports);

};
