new function() {

    bt.package(this, {
        name:    "phoneType",
        imports: "bt,miruken.mvc",
        exports: "PhoneTypeController"
    });

    eval(this.imports);

    const PhoneTypeController = Controller.extend({
        $properties: {
            phoneType: []
        },

        showPhoneType(data) {
            return PhoneTypeFeature(this.io).phoneType(data.id).then(phoneType => {
                this.phoneType = phoneType;
                return ViewRegion(this.io).show("app/phoneTypes/phoneType");
            });
        },
        goToEdit() {
            return bt.phoneType.EditPhoneTypeController(this.io)
                .next(ctrl => ctrl.showEditPhoneType(this.phoneType));
        }
    });

    eval(this.exports);

};
