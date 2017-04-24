new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "CoursesController"
    });

    eval(this.imports);

    const CoursesController = Controller.extend({
        $properties: {
            courses:   [],
            dtOptions: undefined,
            dtColumns: undefined
        },
        $inject: ["DTOptionsBuilder", "DTColumnBuilder", "$scope", Context],
        constructor(optionsBuilder, columnBuilder, $scope, context) {
            this.$scope = $scope;
            this.context = context;

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
                })
                .withOption('rowCallback', this.rowCallback.bind(this));

            this.dtColumns = [
                columnBuilder.newColumn('name').withTitle('Name'),
                columnBuilder.newColumn('description').withTitle('Description')
            ];
        },
        rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            // Unbind first in order to avoid any duplicate handler
            //(see https://github.com/l-lin/angular-datatables/issues/87)
            $('td', nRow).unbind('click');
            $('td', nRow).bind('click', () => {
                this.$scope.$apply(() => {
                    this.rowSelected(aData);
                });
            });
            return nRow;
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
