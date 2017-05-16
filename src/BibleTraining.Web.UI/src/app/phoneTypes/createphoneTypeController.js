new function() {

    bt.package(this, {
        name:    "phoneType",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreatePhoneTypeController"
    });

    eval(this.imports);

    const CreatePhoneTypeController = Controller.extend({
        $properties:{
            title:      "Create PhoneType",
            buttonText: "Create PhoneType",
            isCreate:   true,
            phoneType: { validate: $nested }
        },
        constructor() {
            this.phoneType = new PhoneType();
        },

        showCreatePhoneType() {
            return ViewRegion(this.io).show("app/phoneTypes/createEditPhoneType");
        },
        save() {
            return PhoneTypeFeature(this.ifValid)
                .createPhoneType(this.phoneType)
                .then(phoneType => bt.phoneType.PhoneTypeController(this.io).next(
                    ctrl => ctrl.showPhoneType({id: this.phoneType.id })));
        },
        cancel() {
            return bt.phoneType.PhoneTypesController(this.io).next(
                ctrl => ctrl.showPhoneTypes());
        }
    });

    eval(this.exports);

};
