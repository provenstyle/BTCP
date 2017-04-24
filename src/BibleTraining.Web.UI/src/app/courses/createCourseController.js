new function() {

    bt.package(this, {
        name:    "course",
        imports: "bt,miruken.mvc,miruken.validate",
        exports: "CreateCourseController"
    });

    eval(this.imports);

    const CreateCourseController = Controller.extend({
        $properties:{
            title:      "Create A Course",
            buttonText: "Create Course",
            course:     { validate: $nested }
        },
        constructor() {
            this.course = new Course();
        },

        showCreateCourse() {
            return ViewRegion(this.io).show("app/courses/createEditCourse");
        },
        save() {
            return CourseFeature(this.ifValid)
                .createCourse(this.course)
                .then(course => CourseController(this.io).next(
                    ctrl => ctrl.showCourse({id: this.course.id })));
        },
        cancel() {
            return CoursesController(this.io).next(
                ctrl => ctrl.showCourses());
        }
    });

    eval(this.exports);

};