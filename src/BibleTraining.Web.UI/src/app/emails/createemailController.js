new function() {

    bt.package(this, {
        name:    "email",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreateEmailController"
    });

    eval(this.imports);

    const CreateEmailController = Controller.extend({
        $properties:{
            title:      "Create Email",
            buttonText: "Create Email",
            isCreate:   true,
            email: { validate: $nested }
        },
        constructor() {
            this.email = new Email();
        },

        showCreateEmail() {
            return ViewRegion(this.io).show("app/emails/createEditEmail");
        },
        save() {
            return EmailFeature(this.ifValid)
                .createEmail(this.email)
                .then(email => bt.email.EmailController(this.io).next(
                    ctrl => ctrl.showEmail({id: this.email.id })));
        },
        cancel() {
            return bt.email.EmailsController(this.io).next(
                ctrl => ctrl.showEmails());
        }
    });

    eval(this.exports);

};
