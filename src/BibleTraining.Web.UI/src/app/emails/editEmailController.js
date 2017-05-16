new function() {

    bt.package(this, {
        name:    "email",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditEmailController"
    });

    eval(this.imports);

    const EditEmailController = Controller.extend({
        $properties: {
            title:      "Edit Email",
            buttonText: "Save Email",
            isEdit:     true,
            email: { validate: $nested }
        },

        showEditEmail(data) {
            return EmailFeature(this.io).email(data.id).then(email => {
                this.email = email;
                return ViewRegion(this.io).show("app/emails/createEditEmail");
            });
        },
        save() {
            return EmailFeature(this.ifValid)
                .updateEmail(this.email)
                .then(email => bt.email.EmailController(this.io).next(
                    ctrl => ctrl.showEmail({id: this.email.id })));
        },
        cancel() {
            return bt.email.EmailController(this.io).next(
                ctrl => ctrl.showEmail({ id: this.email.id }));
        },
        remove() {
            return EmailFeature(this.io
                .$confirm(`Delete Email "${this.email.name}"?`))
                .removeEmail(this.email).then(() =>
                    bt.email.EmailsController(this.io).next(ctrl => ctrl.showEmails()));
        }
    });

    eval(this.exports);

};
