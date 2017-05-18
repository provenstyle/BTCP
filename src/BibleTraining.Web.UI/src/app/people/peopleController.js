new function() {

    bt.package(this, {
        name:    "person",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "PeopleController"
    });

    eval(this.imports);

    const PeopleController = TableController.extend({
        configure(table) {
            table
                .url("api/people")
                .text("firstName", "First Name")
                .text("lastName",  "Last Name")
                .text("gender",    "Gender")
                .text("birthDate", "Birth Date")
                .text("bio",       "Bio")
                .text("image",     "Image")
                .rowSelected(data => {
                    bt.person.PersonController(this.context).next(ctrl => ctrl.showPerson(data));
                });
        },
        showPeople() {
            return ViewRegion(this.io).show("app/people/people");
        },
        goToCreate() {
            bt.person.CreatePersonController(this.io).next(ctrl => ctrl.showCreatePerson());
        },
        rowSelected(data) {
           bt.person.PersonController(this.context).next(ctrl => ctrl.showPerson(data));
        }
    });

    eval(this.exports);

};
