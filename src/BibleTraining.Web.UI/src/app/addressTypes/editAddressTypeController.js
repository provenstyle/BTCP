new function() {

    bt.package(this, {
        name:    "addressType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditAddressTypeController"
    });

    eval(this.imports);

    const EditAddressTypeController = Controller.extend({
        $properties: {
            title:      "Edit AddressType",
            buttonText: "Save AddressType",
            isEdit:     true,
            addressType: { validate: $nested }
        },

        showEditAddressType(data) {
            return AddressTypeFeature(this.io).addressType(data.id).then(addressType => {
                this.addressType = addressType;
                return ViewRegion(this.io).show("app/addressTypes/createEditAddressType");
            });
        },
        save() {
            return AddressTypeFeature(this.ifValid)
                .updateAddressType(this.addressType)
                .then(addressType => bt.addressType.AddressTypeController(this.io).next(
                    ctrl => ctrl.showAddressType({id: this.addressType.id })));
        },
        cancel() {
            return bt.addressType.AddressTypeController(this.io).next(
                ctrl => ctrl.showAddressType({ id: this.addressType.id }));
        },
        remove() {
            return AddressTypeFeature(this.io
                .$confirm(`Delete AddressType "${this.addressType.name}"?`))
                .removeAddressType(this.addressType).then(() =>
                    bt.addressType.AddressTypesController(this.io).next(ctrl => ctrl.showAddressTypes()));
        }
    });

    eval(this.exports);

};
