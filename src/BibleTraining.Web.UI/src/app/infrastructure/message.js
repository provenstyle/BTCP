new function() {

    base2.package(this, {
        name:    "em",
        imports: "miruken",
        exports: "MessageLevel,Messaging"
    });

    eval(this.imports);

    const MessageLevel = Enum({
        Info:    1,
        Warning: 2,
        Error:   3
    });

    const Messaging = StrictProtocol.extend({
        showMessage(message, level) {}
    });

    eval(this.exports);

};
