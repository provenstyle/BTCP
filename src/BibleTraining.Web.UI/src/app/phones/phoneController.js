new function() {

    bt.package(this, {
        name:    "phone",
        imports: "bt,miruken.mvc",
        exports: "PhoneController"
    });

    eval(this.imports);

    const PhoneController = Controller.extend({
        $properties: {
            phone: []
        },

        showPhone(data) {
            return PhoneFeature(this.io).phone(data.id).then(phone => {
                this.phone = phone;
                return ViewRegion(this.io).show("app/phones/phone");
            });
        },
        goToEdit() {
            return bt.phone.EditPhoneController(this.io)
                .next(ctrl => ctrl.showEditPhone(this.phone));
        }
    });

    eval(this.exports);

};
