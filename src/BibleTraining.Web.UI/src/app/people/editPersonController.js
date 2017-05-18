new function() {

    bt.package(this, {
        name:    "person",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditPersonController"
    });

    eval(this.imports);

    const EditPersonController = Controller.extend({
        $properties: {
            title:      "Edit Person",
            buttonText: "Save Person",
            isEdit:     true,
            person: { validate: $nested }
        },

        showEditPerson(data) {
            return PersonFeature(this.io).person(data.id).then(person => {
                this.person = person;
                return ViewRegion(this.io).show("app/people/createEditPerson");
            });
        },
        save() {
            return PersonFeature(this.ifValid)
                .updatePerson(this.person)
                .then(person => bt.person.PersonController(this.io).next(
                    ctrl => ctrl.showPerson({id: this.person.id })));
        },
        cancel() {
            return bt.person.PersonController(this.io).next(
                ctrl => ctrl.showPerson({ id: this.person.id }));
        },
        remove() {
            return PersonFeature(this.io
                .$confirm(`Delete Person "${this.person.name}"?`))
                .removePerson(this.person).then(() =>
                    bt.person.PeopleController(this.io).next(ctrl => ctrl.showPeople()));
        }
    });

    eval(this.exports);

};
