new function() {

    bt.package(this, {
        name:    "phoneType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditPhoneTypeController"
    });

    eval(this.imports);

    const EditPhoneTypeController = Controller.extend({
        $properties: {
            title:      "Edit PhoneType",
            buttonText: "Save PhoneType",
            isEdit:     true,
            phoneType: { validate: $nested }
        },

        showEditPhoneType(data) {
            return PhoneTypeFeature(this.io).phoneType(data.id).then(phoneType => {
                this.phoneType = phoneType;
                return ViewRegion(this.io).show("app/phoneTypes/createEditPhoneType");
            });
        },
        save() {
            return PhoneTypeFeature(this.ifValid)
                .updatePhoneType(this.phoneType)
                .then(phoneType => bt.phoneType.PhoneTypeController(this.io).next(
                    ctrl => ctrl.showPhoneType({id: this.phoneType.id })));
        },
        cancel() {
            return bt.phoneType.PhoneTypeController(this.io).next(
                ctrl => ctrl.showPhoneType({ id: this.phoneType.id }));
        },
        remove() {
            return PhoneTypeFeature(this.io
                .$confirm(`Delete PhoneType "${this.phoneType.name}"?`))
                .removePhoneType(this.phoneType).then(() =>
                    bt.phoneType.PhoneTypesController(this.io).next(ctrl => ctrl.showPhoneTypes()));
        }
    });

    eval(this.exports);

};
