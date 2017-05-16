new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "Address"
    });

    eval(this.imports);

    const Address = Model.extend({
        $properties: {
            id:          undefined,
            name:        { validate: $required },
            description: undefined,
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    eval(this.exports);

};
