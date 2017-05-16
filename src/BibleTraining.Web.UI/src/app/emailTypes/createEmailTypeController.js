new function() {

    bt.package(this, {
        name:    "emailType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreateEmailTypeController"
    });

    eval(this.imports);

    const CreateEmailTypeController = Controller.extend({
        $properties:{
            title:      "Create EmailType",
            buttonText: "Create EmailType",
            isCreate:   true,
            emailType: { validate: $nested }
        },
        constructor() {
            this.emailType = new EmailType();
        },

        showCreateEmailType() {
            return ViewRegion(this.io).show("app/emailTypes/createEditEmailType");
        },
        save() {
            return EmailTypeFeature(this.ifValid)
                .createEmailType(this.emailType)
                .then(emailType => bt.emailType.EmailTypeController(this.io).next(
                    ctrl => ctrl.showEmailType({id: this.emailType.id })));
        },
        cancel() {
            return bt.emailType.EmailTypesController(this.io).next(
                ctrl => ctrl.showEmailTypes());
        }
    });

    eval(this.exports);

};
