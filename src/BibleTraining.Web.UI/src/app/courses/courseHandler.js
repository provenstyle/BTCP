new function(){

    bt.package(this, {
        name:    "course",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "CourseHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const CourseResult = Result.extend({
        "$type": "BibleTraining.Api.Course.CourseResult, BibleTraining.Api",
        $properties: {
            courses: { map: Course }
        }
    });

    const GetCourses = Request(CourseResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.Course.GetCourses, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CourseHandler = CallbackHandler.extend(CourseFeature, {
        courses() {
            return ServiceBus($composer).process(new GetCourses()).then(data => {
                return data.courses;
            });
        },
        course(id) {
            return ServiceBus($composer).process(new GetCourses({ids: [id]}));
        },
        createCourse(course) {
            course.id = nextId();
            courses.push(course);
            return Promise.resolve(course);
        },
        deleteCourse(course) {
        },
        updateCourse(course) {
            const existing = courses.find(item => item.id === course.id);
            if (existing) {
                let result = existing.fromData(course);
            }
            return Promise.resolve(existing);
        }
    });


    eval(this.exports);

};
