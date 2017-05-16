new function() {

    bt.package(this, {
        name:    "email",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "EmailsController"
    });

    eval(this.imports);

    const EmailsController = TableController.extend({
        configure(table) {
            table
                .url("api/emails")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.email.EmailController(this.context).next(ctrl => ctrl.showEmail(data));
                });
        },
        showEmails() {
            return ViewRegion(this.io).show("app/emails/emails");
        },
        goToCreate() {
            bt.email.CreateEmailController(this.io).next(ctrl => ctrl.showCreateEmail());
        },
        rowSelected(data) {
           bt.email.EmailController(this.context).next(ctrl => ctrl.showEmail(data));
        }
    });

    eval(this.exports);

};
