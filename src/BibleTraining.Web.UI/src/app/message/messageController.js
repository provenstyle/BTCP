new function() {

    base2.package(this, {
        name:    "bt",
        imports: "miruken.mvc",
        exports: "MessageController"
    });

    eval(this.imports);

    const MessageController = Controller.extend(Messaging, {
        $properties: {
            message: null
        },

        $inject: ["message"],
        constructor(message) {
            this.message = message;
        },

        showMessage(message, level) {
            this.message = message;
        }
    });

    eval(this.exports);

};
