new function () {

    base2.package(this, {
        name:     "bt",
        ngModule: [
            "ui.router",
            "ngMessages",
            "ngLocalize",
            "ngLocalize.InstalledLanguages",
            "localytics.directives",
            "inspinia",
            "ui.bootstrap",
            "ui.footable"
        ]
    });

    eval(this.imports);

};
