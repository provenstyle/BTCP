new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken",
        exports: "CourseFeature"
    });

    eval(this.imports);

    const CourseFeature = StrictProtocol.extend(Resolving, {
        createCourse(course)   {},
        course(id)             {},
        courses()              {},
        removeCourse(course)   {},
        updateCourse(course)   {},
        editCourse(course)     {}
    });

    eval(this.exports);

};
