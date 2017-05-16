new function() {

    bt.package(this, {
        name:    "address",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditAddressController"
    });

    eval(this.imports);

    const EditAddressController = Controller.extend({
        $properties: {
            title:      "Edit Address",
            buttonText: "Save Address",
            isEdit:     true,
            address: { validate: $nested }
        },

        showEditAddress(data) {
            return AddressFeature(this.io).address(data.id).then(address => {
                this.address = address;
                return ViewRegion(this.io).show("app/addresses/createEditAddress");
            });
        },
        save() {
            return AddressFeature(this.ifValid)
                .updateAddress(this.address)
                .then(address => bt.address.AddressController(this.io).next(
                    ctrl => ctrl.showAddress({id: this.address.id })));
        },
        cancel() {
            return bt.address.AddressController(this.io).next(
                ctrl => ctrl.showAddress({ id: this.address.id }));
        },
        remove() {
            return AddressFeature(this.io
                .$confirm(`Delete Address "${this.address.name}"?`))
                .removeAddress(this.address).then(() =>
                    bt.address.AddressesController(this.io).next(ctrl => ctrl.showAddresses()));
        }
    });

    eval(this.exports);

};
