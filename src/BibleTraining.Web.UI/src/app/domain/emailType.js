new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "EmailType"
    });

    eval(this.imports);

    const EmailType = Model.extend({
        $properties: {
            id:          undefined,
            name:        { validate: $required },
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    eval(this.exports);

};
