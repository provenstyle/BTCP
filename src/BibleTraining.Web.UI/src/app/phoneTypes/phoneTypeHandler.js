new function(){

    bt.package(this, {
        name:    "phoneType",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "PhoneTypeHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const PhoneTypeResult = Result.extend({
        "$type": "BibleTraining.Api.PhoneType.PhoneTypeResult, BibleTraining.Api",
        $properties: {
            phoneTypes: { map: PhoneType }
        }
    });

    const GetPhoneTypes = Request(PhoneTypeResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.PhoneType.GetPhoneTypes, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreatePhoneType = Request(PhoneType).extend({
        $properties: {
            "$type": "BibleTraining.Api.PhoneType.CreatePhoneType, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdatePhoneType = Request(PhoneType).extend({
        $properties: {
            "$type": "BibleTraining.Api.PhoneType.UpdatePhoneType, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemovePhoneType = Request(PhoneType).extend({
        $properties: {
            "$type": "BibleTraining.Api.PhoneType.RemovePhoneType, BibleTraining.Api",
            resource: undefined
        }
    });

    const PhoneTypeHandler = CallbackHandler.extend(PhoneTypeFeature, {
        phoneTypes() {
            return ServiceBus($composer).process(new GetPhoneTypes()).then(data => {
                return data.phoneTypes;
            });
        },
        phoneType(id) {
            return ServiceBus($composer).process(new GetPhoneTypes({ids: [id]})).then(data => {
                return (data.phoneTypes && data.phoneTypes.length > 0)
                    ? data.phoneTypes[0]
                    : undefined;
            });
        },
        createPhoneType(phoneType) {
            const config = $composer.resolve(Configuration);
            phoneType.createdBy = phoneType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreatePhoneType({ resource: phoneType })).then(data => {
                return phoneType.fromData(data);
            });
        },
        updatePhoneType(phoneType) {
            const config = $composer.resolve(Configuration);
            phoneType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdatePhoneType({ resource: phoneType })).then(data => {
                return phoneType.fromData(data);
            });
        },
        removePhoneType(phoneType) {
            const config = $composer.resolve(Configuration);
            phoneType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemovePhoneType({ resource: phoneType })).then(data => {
                return phoneType.fromData(data);
            });
        }
    });

    eval(this.exports);

};
