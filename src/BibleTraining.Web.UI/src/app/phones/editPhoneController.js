new function() {

    bt.package(this, {
        name:    "phone",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditPhoneController"
    });

    eval(this.imports);

    const EditPhoneController = Controller.extend({
        $properties: {
            title:      "Edit Phone",
            buttonText: "Save Phone",
            isEdit:     true,
            phone: { validate: $nested }
        },

        showEditPhone(data) {
            return PhoneFeature(this.io).phone(data.id).then(phone => {
                this.phone = phone;
                return ViewRegion(this.io).show("app/phones/createEditPhone");
            });
        },
        save() {
            return PhoneFeature(this.ifValid)
                .updatePhone(this.phone)
                .then(phone => bt.phone.PhoneController(this.io).next(
                    ctrl => ctrl.showPhone({id: this.phone.id })));
        },
        cancel() {
            return bt.phone.PhoneController(this.io).next(
                ctrl => ctrl.showPhone({ id: this.phone.id }));
        },
        remove() {
            return PhoneFeature(this.io
                .$confirm(`Delete Phone "${this.phone.name}"?`))
                .removePhone(this.phone).then(() =>
                    bt.phone.PhonesController(this.io).next(ctrl => ctrl.showPhones()));
        }
    });

    eval(this.exports);

};
