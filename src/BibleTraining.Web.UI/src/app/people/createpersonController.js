new function() {

    bt.package(this, {
        name:    "person",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreatePersonController"
    });

    eval(this.imports);

    const CreatePersonController = Controller.extend({
        $properties:{
            title:      "Create Person",
            buttonText: "Create Person",
            isCreate:   true,
            person: { validate: $nested }
        },
        constructor() {
            this.person = new Person();
        },

        showCreatePerson() {
            return ViewRegion(this.io).show("app/people/createEditPerson");
        },
        save() {
            return PersonFeature(this.ifValid)
                .createPerson(this.person)
                .then(person => bt.person.PersonController(this.io).next(
                    ctrl => ctrl.showPerson({id: this.person.id })));
        },
        cancel() {
            return bt.person.PeopleController(this.io).next(
                ctrl => ctrl.showPeople());
        }
    });

    eval(this.exports);

};
