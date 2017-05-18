new function() {

    bt.package(this, {
        name:    "person",
        imports: "bt,miruken.mvc",
        exports: "PersonController"
    });

    eval(this.imports);

    const PersonController = Controller.extend({
        $properties: {
            person: []
        },

        showPerson(data) {
            return PersonFeature(this.io).person(data.id).then(person => {
                this.person = person;
                return ViewRegion(this.io).show("app/people/person");
            });
        },
        goToEdit() {
            return bt.person.EditPersonController(this.io)
                .next(ctrl => ctrl.showEditPerson(this.person));
        }
    });

    eval(this.exports);

};
