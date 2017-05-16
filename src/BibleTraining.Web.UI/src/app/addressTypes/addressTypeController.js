new function() {

    bt.package(this, {
        name:    "addressType",
        imports: "bt,miruken.mvc",
        exports: "AddressTypeController"
    });

    eval(this.imports);

    const AddressTypeController = Controller.extend({
        $properties: {
            addressType: []
        },

        showAddressType(data) {
            return AddressTypeFeature(this.io).addressType(data.id).then(addressType => {
                this.addressType = addressType;
                return ViewRegion(this.io).show("app/addressTypes/addressType");
            });
        },
        goToEdit() {
            return bt.addressType.EditAddressTypeController(this.io)
                .next(ctrl => ctrl.showEditAddressType(this.addressType));
        }
    });

    eval(this.exports);

};
