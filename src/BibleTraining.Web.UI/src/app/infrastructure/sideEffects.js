new function() {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.callback,miruken.mvc",
        exports: "SideEffects,SideEffectsHandler"
    });

    eval(this.imports);

    const CONFIRM_TITLE = "Confirm",
          YES           = "Yes",
          NO            = "No";

    const SideEffects = StrictProtocol.extend(Resolving, {
        confirm(message) {}
    });

    const SideEffectsHandler = CallbackHandler.extend(SideEffects, {
        confirm(message) {
            return ViewRegion($composer.modal({
                css:   "confirm-modal",
                title: CONFIRM_TITLE,
                buttons: [
                    { text: YES, css: "btn-primary" },
                    { text: NO,  css: "btn-default" }
                ]
            })
            ).show({ template: `<p>${message}</p>` })
             .then(layer  => layer.modalResult.then(result => {
                 layer.dispose();
                 const button = result.button;
                 return !!button && button.text === YES;
            }));
        }
    });

    CallbackHandler.implement({
        $confirm(message) {
            return this.aspect((_, composer) =>
                SideEffects(composer).confirm(message));
        }
    });

    eval(this.exports);

};
