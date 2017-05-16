new function(){

    bt.package(this, {
        name:    "email",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "EmailHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const EmailResult = Result.extend({
        "$type": "BibleTraining.Api.Email.EmailResult, BibleTraining.Api",
        $properties: {
            emails: { map: Email }
        }
    });

    const GetEmails = Request(EmailResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.Email.GetEmails, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreateEmail = Request(Email).extend({
        $properties: {
            "$type": "BibleTraining.Api.Email.CreateEmail, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdateEmail = Request(Email).extend({
        $properties: {
            "$type": "BibleTraining.Api.Email.UpdateEmail, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemoveEmail = Request(Email).extend({
        $properties: {
            "$type": "BibleTraining.Api.Email.RemoveEmail, BibleTraining.Api",
            resource: undefined
        }
    });

    const EmailHandler = CallbackHandler.extend(EmailFeature, {
        emails() {
            return ServiceBus($composer).process(new GetEmails()).then(data => {
                return data.emails;
            });
        },
        email(id) {
            return ServiceBus($composer).process(new GetEmails({ids: [id]})).then(data => {
                return (data.emails && data.emails.length > 0)
                    ? data.emails[0]
                    : undefined;
            });
        },
        createEmail(email) {
            const config = $composer.resolve(Configuration);
            email.createdBy = email.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreateEmail({ resource: email })).then(data => {
                return email.fromData(data);
            });
        },
        updateEmail(email) {
            const config = $composer.resolve(Configuration);
            email.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdateEmail({ resource: email })).then(data => {
                return email.fromData(data);
            });
        },
        removeEmail(email) {
            const config = $composer.resolve(Configuration);
            email.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemoveEmail({ resource: email })).then(data => {
                return email.fromData(data);
            });
        }
    });

    eval(this.exports);

};
