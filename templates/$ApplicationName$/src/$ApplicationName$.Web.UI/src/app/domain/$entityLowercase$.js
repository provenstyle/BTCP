new function () {

    base2.package(this, {
        name:    "$ngApp$",
        imports: "miruken,miruken.mvc,miruken.validate",
        exports: "$Entity$"
    });

    eval(this.imports);

    const $Entity$ = Model.extend({
        $properties: {
            id:          undefined,
            name:        { validate: $required },
            createdBy:   undefined,
            modifiedBy:  undefined
        }
    });

    eval(this.exports);

};
