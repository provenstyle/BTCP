//import "./playerFeature.js";
//import "./createPlayerController.js";

new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken.mvc,miruken.ng,bt",
        exports: "CoursesController"
    });

    eval(this.imports);

    const CoursesController = Controller.extend({
        $properties: {
            courses:   [],
            dtOptions: undefined,
            dtColumns: undefined
        },
        $inject: ["DTOptionsBuilder", "DTColumnBuilder", $appContext],
        constructor(optionsBuilder, columnBuilder, appContext) {
            this.dtOptions = optionsBuilder
                .fromFnPromise(() => {
                    return CourseFeature(appContext).courses()
                        .then(courses => {
                            return courses;
                        });
                })
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    {extend: 'copy'},
                    {extend: 'csv'},
                    {extend: 'excel', title: 'ExampleFile'},
                    {extend: 'pdf', title: 'ExampleFile'},

                    {extend: 'print',
                        customize: function (win){
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ]);
            this.dtColumns = [
                columnBuilder.newColumn('name').withTitle('Name'),
                columnBuilder.newColumn('description').withTitle('Description')
            ];
        },
        getCourses() {
            return CourseFeature(this.io).courses()
                .then(courses => this.courses = courses);
        },
        showCourses() {
            return ViewRegion(this.io).show("app/courses/courses");
        }
    });

    eval(this.exports);

};
