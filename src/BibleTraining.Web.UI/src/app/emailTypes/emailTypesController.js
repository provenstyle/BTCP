new function() {

    bt.package(this, {
        name:    "emailType",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "EmailTypesController"
    });

    eval(this.imports);

    const EmailTypesController = TableController.extend({
        configure(table) {
            table
                .url("api/emailTypes")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.emailType.EmailTypeController(this.context).next(ctrl => ctrl.showEmailType(data));
                });
        },
        showEmailTypes() {
            return ViewRegion(this.io).show("app/emailTypes/emailTypes");
        },
        goToCreate() {
            bt.emailType.CreateEmailTypeController(this.io).next(ctrl => ctrl.showCreateEmailType());
        },
        rowSelected(data) {
           bt.emailType.EmailTypeController(this.context).next(ctrl => ctrl.showEmailType(data));
        }
    });

    eval(this.exports);

};
