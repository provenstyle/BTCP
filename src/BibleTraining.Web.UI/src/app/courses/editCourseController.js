new function() {

    bt.package(this, {
        name:    "course",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "EditCourseController"
    });

    eval(this.imports);

    const EditCourseController = Controller.extend({
        $properties: {
            title:      "Edit A Course",
            buttonText: "Save Course",
            course:     { validate: $nested }
        },

        showEditCourse(data) {
            return CourseFeature(this.io).course(data.id).then(course => {
                this.course = course;
                return ViewRegion(this.io).show("app/courses/createEditCourse");
            });
        },
        save() {
            return CourseFeature(this.ifValid)
                .updateCourse(this.course)
                .then(course => CourseController(this.io).next(
                    ctrl => ctrl.showCourse({id: this.course.id })));
        },
        cancel() {
            return CourseController(this.io).next(
                ctrl => ctrl.showCourse({ id: this.course.id }));
        }
    });

    eval(this.exports);

};
