new function() {

    bt.package(this, {
        name:    "emailType",
        imports: "bt,miruken.mvc",
        exports: "EmailTypeController"
    });

    eval(this.imports);

    const EmailTypeController = Controller.extend({
        $properties: {
            emailType: []
        },

        showEmailType(data) {
            return EmailTypeFeature(this.io).emailType(data.id).then(emailType => {
                this.emailType = emailType;
                return ViewRegion(this.io).show("app/emailTypes/emailType");
            });
        },
        goToEdit() {
            return bt.emailType.EditEmailTypeController(this.io)
                .next(ctrl => ctrl.showEditEmailType(this.emailType));
        }
    });

    eval(this.exports);

};
