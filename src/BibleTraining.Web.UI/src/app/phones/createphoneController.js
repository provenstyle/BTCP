new function() {

    bt.package(this, {
        name:    "phone",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreatePhoneController"
    });

    eval(this.imports);

    const CreatePhoneController = Controller.extend({
        $properties:{
            title:      "Create Phone",
            buttonText: "Create Phone",
            isCreate:   true,
            phone: { validate: $nested }
        },
        constructor() {
            this.phone = new Phone();
        },

        showCreatePhone() {
            return ViewRegion(this.io).show("app/phones/createEditPhone");
        },
        save() {
            return PhoneFeature(this.ifValid)
                .createPhone(this.phone)
                .then(phone => bt.phone.PhoneController(this.io).next(
                    ctrl => ctrl.showPhone({id: this.phone.id })));
        },
        cancel() {
            return bt.phone.PhonesController(this.io).next(
                ctrl => ctrl.showPhones());
        }
    });

    eval(this.exports);

};
