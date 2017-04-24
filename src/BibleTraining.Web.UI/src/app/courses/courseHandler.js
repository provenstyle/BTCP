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

    const CreateCourse = Request(Course).extend({
        $properties: {
            "$type": "BibleTraining.Api.Course.CreateCourse, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdateCourse = Request(Course).extend({
        $properties: {
            "$type": "BibleTraining.Api.Course.UpdateCourse, BibleTraining.Api",
            resource: undefined
        }
    });

    const CourseHandler = CallbackHandler.extend(CourseFeature, {
        courses() {
            return ServiceBus($composer).process(new GetCourses()).then(data => {
                return data.courses;
            });
        },
        course(id) {
            return ServiceBus($composer).process(new GetCourses({ids: [id]})).then(data => {
                return (data.courses && data.courses.length > 0)
                    ? data.courses[0]
                    : undefined;
            });
        },
        createCourse(course) {
            const config = $composer.resolve(Configuration);
            course.createdBy = course.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreateCourse({ resource: course })).then(data => {
                return course.fromData(data);
            });
        },
        deleteCourse(course) {
        },
        updateCourse(course) {
            const config = $composer.resolve(Configuration);
            course.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdateCourse({ resource: course })).then(data => {
                return course.fromData(data);
            });
        }
    });


    eval(this.exports);

};
