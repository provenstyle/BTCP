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
        constructor(optionsBuilder, columnBuilder) {
            this.dtOptions = optionsBuilder.newOptions()
                .withOption('ajax', {
                    url: 'api/courses',
                    type: 'POST'
                })
                .withDataProp('data')
                .withOption('processing', true)
                .withOption('serverSide', true)
                .withPaginationType('full_numbers')
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
                ])
                .withLightColumnFilter({
                    '0' : { html: 'input', type: 'text', attr: { class: 'form-control' }},
                    '1' : { html: 'input', type: 'text', attr: { class: 'form-control' }}
                });

            this.dtColumns = [
                columnBuilder.newColumn('name').withTitle('Name'),
                columnBuilder.newColumn('description').withTitle('Description')
            ];
        },
        showCourses() {
            return ViewRegion(this.io).show("app/courses/courses");
        },
        goToCreate() {
            bt.course.CreateCourseController(this.io).next(ctrl => ctrl.showCreateCourse());
        }
    });

    eval(this.exports);

};
