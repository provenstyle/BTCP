new function(){

    bt.package(this, {
        name:    "phone",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "PhoneHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const PhoneResult = Result.extend({
        "$type": "BibleTraining.Api.Phone.PhoneResult, BibleTraining.Api",
        $properties: {
            phones: { map: Phone }
        }
    });

    const GetPhones = Request(PhoneResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.Phone.GetPhones, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreatePhone = Request(Phone).extend({
        $properties: {
            "$type": "BibleTraining.Api.Phone.CreatePhone, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdatePhone = Request(Phone).extend({
        $properties: {
            "$type": "BibleTraining.Api.Phone.UpdatePhone, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemovePhone = Request(Phone).extend({
        $properties: {
            "$type": "BibleTraining.Api.Phone.RemovePhone, BibleTraining.Api",
            resource: undefined
        }
    });

    const PhoneHandler = CallbackHandler.extend(PhoneFeature, {
        phones() {
            return ServiceBus($composer).process(new GetPhones()).then(data => {
                return data.phones;
            });
        },
        phone(id) {
            return ServiceBus($composer).process(new GetPhones({ids: [id]})).then(data => {
                return (data.phones && data.phones.length > 0)
                    ? data.phones[0]
                    : undefined;
            });
        },
        createPhone(phone) {
            const config = $composer.resolve(Configuration);
            phone.createdBy = phone.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreatePhone({ resource: phone })).then(data => {
                return phone.fromData(data);
            });
        },
        updatePhone(phone) {
            const config = $composer.resolve(Configuration);
            phone.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdatePhone({ resource: phone })).then(data => {
                return phone.fromData(data);
            });
        },
        removePhone(phone) {
            const config = $composer.resolve(Configuration);
            phone.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemovePhone({ resource: phone })).then(data => {
                return phone.fromData(data);
            });
        }
    });

    eval(this.exports);

};
