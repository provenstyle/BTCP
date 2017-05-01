new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "CoursesController"
    });

    eval(this.imports);

    const CoursesController = TableController.extend({
        configure(table) {
            table
                .url("api/courses")
                .text("name", "Name")
                .text("description", "Description")
                .rowSelected(data => {
                    bt.course.CourseController(this.context).next(ctrl => ctrl.showCourse(data));
                });
        },
        showCourses() {
            return ViewRegion(this.io).show("app/courses/courses");
        },
        goToCreate() {
            bt.course.CreateCourseController(this.io).next(ctrl => ctrl.showCreateCourse());
        },
        rowSelected(data) {
           bt.course.CourseController(this.context).next(ctrl => ctrl.showCourse(data));
        }
    });

    eval(this.exports);

};
