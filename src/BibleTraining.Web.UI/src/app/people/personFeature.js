new function() {

    bt.package(this, {
        name:    "person",
        imports: "miruken",
        exports: "PersonFeature"
    });

    eval(this.imports);

    const PersonFeature = StrictProtocol.extend(Resolving, {
        createPerson(person)   {},
        person(id)             {},
        people()              {},
        removePerson(person)   {},
        updatePerson(person)   {},
        editPerson(person)     {}
    });

    eval(this.exports);

};
