new function() {

    bt.package(this, {
        name:    "emailType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditEmailTypeController"
    });

    eval(this.imports);

    const EditEmailTypeController = Controller.extend({
        $properties: {
            title:      "Edit A EmailType",
            buttonText: "Save EmailType",
            showDelete: true,
            emailType:     { validate: $nested }
        },

        showEditEmailType(data) {
            return EmailTypeFeature(this.io).emailType(data.id).then(emailType => {
                this.emailType = emailType;
                return ViewRegion(this.io).show("app/emailTypes/createEditEmailType");
            });
        },
        save() {
            return EmailTypeFeature(this.ifValid)
                .updateEmailType(this.emailType)
                .then(emailType => bt.emailType.EmailTypeController(this.io).next(
                    ctrl => ctrl.showEmailType({id: this.emailType.id })));
        },
        cancel() {
            return bt.emailType.EmailTypeController(this.io).next(
                ctrl => ctrl.showEmailType({ id: this.emailType.id }));
        },
        remove() {
            return EmailTypeFeature(this.io
                .$confirm(`Delete EmailType "${this.emailType.name}"?`))
                .removeEmailType(this.emailType).then(() =>
                    bt.emailType.EmailTypesController(this.io).next(ctrl => ctrl.showEmailTypes()));
        }
    });

    eval(this.exports);

};
