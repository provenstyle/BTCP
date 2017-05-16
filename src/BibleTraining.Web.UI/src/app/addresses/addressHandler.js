new function(){

    bt.package(this, {
        name:    "address",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "AddressHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const AddressResult = Result.extend({
        "$type": "BibleTraining.Api.Address.AddressResult, BibleTraining.Api",
        $properties: {
            addresses: { map: Address }
        }
    });

    const GetAddresses = Request(AddressResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.Address.GetAddresses, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreateAddress = Request(Address).extend({
        $properties: {
            "$type": "BibleTraining.Api.Address.CreateAddress, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdateAddress = Request(Address).extend({
        $properties: {
            "$type": "BibleTraining.Api.Address.UpdateAddress, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemoveAddress = Request(Address).extend({
        $properties: {
            "$type": "BibleTraining.Api.Address.RemoveAddress, BibleTraining.Api",
            resource: undefined
        }
    });

    const AddressHandler = CallbackHandler.extend(AddressFeature, {
        addresses() {
            return ServiceBus($composer).process(new GetAddresses()).then(data => {
                return data.addresses;
            });
        },
        address(id) {
            return ServiceBus($composer).process(new GetAddresses({ids: [id]})).then(data => {
                return (data.addresses && data.addresses.length > 0)
                    ? data.addresses[0]
                    : undefined;
            });
        },
        createAddress(address) {
            const config = $composer.resolve(Configuration);
            address.createdBy = address.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreateAddress({ resource: address })).then(data => {
                return address.fromData(data);
            });
        },
        updateAddress(address) {
            const config = $composer.resolve(Configuration);
            address.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdateAddress({ resource: address })).then(data => {
                return address.fromData(data);
            });
        },
        removeAddress(address) {
            const config = $composer.resolve(Configuration);
            address.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemoveAddress({ resource: address })).then(data => {
                return address.fromData(data);
            });
        }
    });

    eval(this.exports);

};
