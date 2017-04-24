//import "./playerFeature.js";
//import "./createPlayerController.js";

new function() {

    bt.package(this, {
        name:    "course",
        imports: "bt,miruken.mvc",
        exports: "CourseController"
    });

    eval(this.imports);

    const CourseController = Controller.extend({
        $properties: {
            course: []
        },

        showCourse(data) {
            return CourseFeature(this.io).course(data.id).then(course => {
                this.course = course;
                return ViewRegion(this.io).show("app/courses/course");
            });
        },
        goToEdit() {
            return bt.course.EditCourseController(this.io)
                .push(ctrl => ctrl.showEditCourse(this.course));
        }
    });

    eval(this.exports);

};
