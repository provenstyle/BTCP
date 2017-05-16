new function() {

    bt.package(this, {
        name:    "address",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreateAddressController"
    });

    eval(this.imports);

    const CreateAddressController = Controller.extend({
        $properties:{
            title:      "Create Address",
            buttonText: "Create Address",
            isCreate:   true,
            address: { validate: $nested }
        },
        constructor() {
            this.address = new Address();
        },

        showCreateAddress() {
            return ViewRegion(this.io).show("app/addresses/createEditAddress");
        },
        save() {
            return AddressFeature(this.ifValid)
                .createAddress(this.address)
                .then(address => bt.address.AddressController(this.io).next(
                    ctrl => ctrl.showAddress({id: this.address.id })));
        },
        cancel() {
            return bt.address.AddressesController(this.io).next(
                ctrl => ctrl.showAddresses());
        }
    });

    eval(this.exports);

};
