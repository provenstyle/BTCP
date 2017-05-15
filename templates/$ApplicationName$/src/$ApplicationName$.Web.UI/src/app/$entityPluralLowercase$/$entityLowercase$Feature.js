new function() {

    bt.package(this, {
        name:    "$entityLowercase$",
        imports: "miruken",
        exports: "$Entity$Feature"
    });

    eval(this.imports);

    const $Entity$Feature = StrictProtocol.extend(Resolving, {
        create$Entity$($entityLowercase$)   {},
        $entityLowercase$(id)             {},
        $entityPluralLowercase$()              {},
        remove$Entity$($entityLowercase$)   {},
        update$Entity$($entityLowercase$)   {},
        edit$Entity$($entityLowercase$)     {}
    });

    eval(this.exports);

};
