new function() {

    bt.package(this, {
        name:    "addressType",
        imports: "miruken",
        exports: "AddressTypeFeature"
    });

    eval(this.imports);

    const AddressTypeFeature = StrictProtocol.extend(Resolving, {
        createAddressType(addressType)   {},
        addressType(id)             {},
        addressTypes()              {},
        removeAddressType(addressType)   {},
        updateAddressType(addressType)   {},
        editAddressType(addressType)     {}
    });

    eval(this.exports);

};
