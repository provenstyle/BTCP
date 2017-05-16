new function() {

    bt.package(this, {
        name:    "email",
        imports: "bt,miruken.mvc",
        exports: "EmailController"
    });

    eval(this.imports);

    const EmailController = Controller.extend({
        $properties: {
            email: []
        },

        showEmail(data) {
            return EmailFeature(this.io).email(data.id).then(email => {
                this.email = email;
                return ViewRegion(this.io).show("app/emails/email");
            });
        },
        goToEdit() {
            return bt.email.EditEmailController(this.io)
                .next(ctrl => ctrl.showEditEmail(this.email));
        }
    });

    eval(this.exports);

};
