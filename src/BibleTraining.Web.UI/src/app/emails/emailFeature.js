new function() {

    bt.package(this, {
        name:    "email",
        imports: "miruken",
        exports: "EmailFeature"
    });

    eval(this.imports);

    const EmailFeature = StrictProtocol.extend(Resolving, {
        createEmail(email)   {},
        email(id)             {},
        emails()              {},
        removeEmail(email)   {},
        updateEmail(email)   {},
        editEmail(email)     {}
    });

    eval(this.exports);

};
