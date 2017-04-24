new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "Course,Key"
    });

    eval(this.imports);

    const Course = Model.extend({
        $properties: {
            id:          undefined,
            name:        { validate: $required },
            description: { validate: $required },
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    const Key = Model.extend({
        $properties: {
            id:   undefined,
            name: undefined
        }
    });

    eval(this.exports);

};
