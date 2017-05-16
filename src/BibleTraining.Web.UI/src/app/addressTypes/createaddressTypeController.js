new function() {

    bt.package(this, {
        name:    "addressType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreateAddressTypeController"
    });

    eval(this.imports);

    const CreateAddressTypeController = Controller.extend({
        $properties:{
            title:      "Create AddressType",
            buttonText: "Create AddressType",
            isCreate:   true,
            addressType: { validate: $nested }
        },
        constructor() {
            this.addressType = new AddressType();
        },

        showCreateAddressType() {
            return ViewRegion(this.io).show("app/addressTypes/createEditAddressType");
        },
        save() {
            return AddressTypeFeature(this.ifValid)
                .createAddressType(this.addressType)
                .then(addressType => bt.addressType.AddressTypeController(this.io).next(
                    ctrl => ctrl.showAddressType({id: this.addressType.id })));
        },
        cancel() {
            return bt.addressType.AddressTypesController(this.io).next(
                ctrl => ctrl.showAddressTypes());
        }
    });

    eval(this.exports);

};
