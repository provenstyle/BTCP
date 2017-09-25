new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "Phone"
    });

    eval(this.imports);

    const Phone = Model.extend({
        $properties: {
            id:          undefined,
            name:        { validate: $required },
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    eval(this.exports);

};
