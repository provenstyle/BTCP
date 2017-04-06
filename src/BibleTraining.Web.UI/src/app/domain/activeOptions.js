new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc",
        exports: "activeOptions,Key"
    });

    eval(this.imports);

    const activeOptions = [
        { id: true,   name: "Yes" },
        { id: false,  name: "No" }];

    const Key = Model.extend({
        $properties: {
            id:   undefined,
            name: undefined
        }
    });

    eval(this.exports);

};
