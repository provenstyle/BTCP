new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc",
        exports: "Course,Key"
    });

    eval(this.imports);

    const Course = Model.extend({
        $properties: {
            name:        undefined,
            description: undefined
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
