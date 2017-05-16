new function() {

    bt.package(this, {
        name:    "address",
        imports: "miruken",
        exports: "AddressFeature"
    });

    eval(this.imports);

    const AddressFeature = StrictProtocol.extend(Resolving, {
        createAddress(address)   {},
        address(id)             {},
        addresses()              {},
        removeAddress(address)   {},
        updateAddress(address)   {},
        editAddress(address)     {}
    });

    eval(this.exports);

};
