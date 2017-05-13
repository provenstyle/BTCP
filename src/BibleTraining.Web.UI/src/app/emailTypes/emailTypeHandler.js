new function(){

    bt.package(this, {
        name:    "emailType",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "EmailTypeHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const EmailTypeResult = Result.extend({
        "$type": "BibleTraining.Api.EmailType.EmailTypeResult, BibleTraining.Api",
        $properties: {
            emailTypes: { map: EmailType }
        }
    });

    const GetEmailTypes = Request(EmailTypeResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.EmailType.GetEmailTypes, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreateEmailType = Request(EmailType).extend({
        $properties: {
            "$type": "BibleTraining.Api.EmailType.CreateEmailType, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdateEmailType = Request(EmailType).extend({
        $properties: {
            "$type": "BibleTraining.Api.EmailType.UpdateEmailType, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemoveEmailType = Request(EmailType).extend({
        $properties: {
            "$type": "BibleTraining.Api.EmailType.RemoveEmailType, BibleTraining.Api",
            resource: undefined
        }
    });

    const EmailTypeHandler = CallbackHandler.extend(EmailTypeFeature, {
        emailTypes() {
            return ServiceBus($composer).process(new GetEmailTypes()).then(data => {
                return data.emailTypes;
            });
        },
        emailType(id) {
            return ServiceBus($composer).process(new GetEmailTypes({ids: [id]})).then(data => {
                return (data.emailTypes && data.emailTypes.length > 0)
                    ? data.emailTypes[0]
                    : undefined;
            });
        },
        createEmailType(emailType) {
            const config = $composer.resolve(Configuration);
            emailType.createdBy = emailType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreateEmailType({ resource: emailType })).then(data => {
                return emailType.fromData(data);
            });
        },
        updateEmailType(emailType) {
            const config = $composer.resolve(Configuration);
            emailType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdateEmailType({ resource: emailType })).then(data => {
                return emailType.fromData(data);
            });
        },
        removeEmailType(emailType) {
            const config = $composer.resolve(Configuration);
            emailType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemoveEmailType({ resource: emailType })).then(data => {
                return emailType.fromData(data);
            });
        }
    });

    eval(this.exports);

};
