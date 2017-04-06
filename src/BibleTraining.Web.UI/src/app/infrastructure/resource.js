new function () {

    base2.package(this, {
        name:    "serviceBus",
        imports: "miruken.mvc",
        exports: "Resource"
    });

    eval(this.imports);

    const Resource = Model.extend({
        $properties: {
            id:         null,
            rowVersion: null,
            created:    null,
            createdBy:  null,
            modified:   null,
            modifiedBy: null
        }
    });

    eval(this.exports);

};
