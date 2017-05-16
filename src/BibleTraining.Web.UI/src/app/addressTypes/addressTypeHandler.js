new function(){

    bt.package(this, {
        name:    "addressType",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "AddressTypeHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const AddressTypeResult = Result.extend({
        "$type": "BibleTraining.Api.AddressType.AddressTypeResult, BibleTraining.Api",
        $properties: {
            addressTypes: { map: AddressType }
        }
    });

    const GetAddressTypes = Request(AddressTypeResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.AddressType.GetAddressTypes, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreateAddressType = Request(AddressType).extend({
        $properties: {
            "$type": "BibleTraining.Api.AddressType.CreateAddressType, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdateAddressType = Request(AddressType).extend({
        $properties: {
            "$type": "BibleTraining.Api.AddressType.UpdateAddressType, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemoveAddressType = Request(AddressType).extend({
        $properties: {
            "$type": "BibleTraining.Api.AddressType.RemoveAddressType, BibleTraining.Api",
            resource: undefined
        }
    });

    const AddressTypeHandler = CallbackHandler.extend(AddressTypeFeature, {
        addressTypes() {
            return ServiceBus($composer).process(new GetAddressTypes()).then(data => {
                return data.addressTypes;
            });
        },
        addressType(id) {
            return ServiceBus($composer).process(new GetAddressTypes({ids: [id]})).then(data => {
                return (data.addressTypes && data.addressTypes.length > 0)
                    ? data.addressTypes[0]
                    : undefined;
            });
        },
        createAddressType(addressType) {
            const config = $composer.resolve(Configuration);
            addressType.createdBy = addressType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreateAddressType({ resource: addressType })).then(data => {
                return addressType.fromData(data);
            });
        },
        updateAddressType(addressType) {
            const config = $composer.resolve(Configuration);
            addressType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdateAddressType({ resource: addressType })).then(data => {
                return addressType.fromData(data);
            });
        },
        removeAddressType(addressType) {
            const config = $composer.resolve(Configuration);
            addressType.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemoveAddressType({ resource: addressType })).then(data => {
                return addressType.fromData(data);
            });
        }
    });

    eval(this.exports);

};
