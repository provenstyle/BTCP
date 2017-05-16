new function() {

    bt.package(this, {
        name:    "phoneType",
        imports: "miruken",
        exports: "PhoneTypeFeature"
    });

    eval(this.imports);

    const PhoneTypeFeature = StrictProtocol.extend(Resolving, {
        createPhoneType(phoneType)   {},
        phoneType(id)             {},
        phoneTypes()              {},
        removePhoneType(phoneType)   {},
        updatePhoneType(phoneType)   {},
        editPhoneType(phoneType)     {}
    });

    eval(this.exports);

};
