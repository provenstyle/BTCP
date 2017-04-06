new function() {

    base2.package(this, {
        name:    "bt",
        imports: "miruken.callback,miruken.context,miruken.mvc," +
                 "miruken.validate,miruken.error",
        exports: "ErrorController"
    });

    eval(this.imports);

    const CONCURRENT_HDR   = "Your changes have not been saved.",
          CONCURRENT_MSG   = "The data was already changed by another user. Click refresh and make your changes again.",
          VALIDATION_HDR   = "Your request failed validation",
          UNAUTHORIZED_HDR = "Unauthorized",
          UNAUTHORIZED_MSG = "You are not authorized to perform this action",
          UNEXPECTED_HDR   = "An unexpected error has occured",
          REFRESH_MSG      = "Try refreshing the page";

    const ErrorController = Controller.extend(Errors, {
        $properties: {
            error:   false,
            heading: "",
            subtext: ""
        },
        $inject: ["$rootScope", Context],
        constructor($rootScope, context) {
            $rootScope.$on("$stateChangeSuccess", () => this.clearError());

            this.extend({
                reportError(error) {
                    if (error instanceof RejectedError ||
                        error instanceof TimeoutError) {
                        $rootScope.$evalAsync();
                        return;
                    }
                    if (error.status === 400) {
                        if (error.data && error.data.failures) {
                            const validating = $composer.resolve(Validating);
                            if (validating) {
                                validating.validationErrors = error.data.failures;
                                return "";
                            }
                        }
                        this.heading = VALIDATION_HDR;
                        this.subtext = REFRESH_MSG;
                    } else if (error.status === 401) {
                        this.heading = UNAUTHORIZED_HDR;
                        this.subtext = UNAUTHORIZED_MSG;
                    } else if (error.status === 409) {
                        this.heading = CONCURRENT_HDR;
                        this.subtext = CONCURRENT_MSG;
                    } else {
                        this.heading = UNEXPECTED_HDR;
                        this.subtext = REFRESH_MSG;
                    }
                    this.error = true;
                    $rootScope.$evalAsync();
                }
            });
        },
        initialize() {
            this.context = this.context.parent.parent;
        },

        clearError() {
            this.error = false;
            this.heading = "";
            this.subtext = "";
        },
        refresh() {
            location.reload();
        }
    });

    eval(this.exports);

};
