new function() {

    bt.package(this, {
        name:    "phone",
        imports: "miruken",
        exports: "PhoneFeature"
    });

    eval(this.imports);

    const PhoneFeature = StrictProtocol.extend(Resolving, {
        createPhone(phone)   {},
        phone(id)             {},
        phones()              {},
        removePhone(phone)   {},
        updatePhone(phone)   {},
        editPhone(phone)     {}
    });

    eval(this.exports);

};
