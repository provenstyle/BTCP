new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "Person"
    });

    eval(this.imports);

    const Person = Model.extend({
        $properties: {
            id:          undefined,
            firstName:   { validate: $required },
            lastName:    { validate: $required },
            gender:      { validate: $required },
            birthdate:   undefined,
            bio:         undefined,
            image:       undefined,
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    eval(this.exports);

};
