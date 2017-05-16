new function() {

    bt.package(this, {
        name:    "emailType",
        imports: "miruken",
        exports: "EmailTypeFeature"
    });

    eval(this.imports);

    const EmailTypeFeature = StrictProtocol.extend(Resolving, {
        createEmailType(emailType)   {},
        emailType(id)             {},
        emailTypes()              {},
        removeEmailType(emailType)   {},
        updateEmailType(emailType)   {},
        editEmailType(emailType)     {}
    });

    eval(this.exports);

};
