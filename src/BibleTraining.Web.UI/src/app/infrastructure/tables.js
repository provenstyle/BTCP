new function() {

    base2.package(this, {
        name:    "bt",
        imports: "miruken.context,miruken.mvc,miruken.ng,bt",
        exports: "TableController"
    });

    eval(this.imports);

    const TableManager = Base.extend({
        constructor(context) {
            const optionsBuilder = context.resolve("DTOptionsBuilder");
            const columnBuilder  = context.resolve("DTColumnBuilder");

            let _options       = undefined,
                _columnOptions = undefined,
                _url           = undefined,
                _rowSelected   = undefined,
                _index         = 0,
                _columns       = [];


            this.extend({
                get options() {
                    return _options ? _options : _options = _buildOptions();
                },
                get columns() {
                    return _columnOptions ? _columnOptions : _columnOptions = _buildColumnOptions();
                },
                url(url) {
                    _url = url;
                    return this;
                },
                text(name, title) {
                    _addColumn(name, title, "text");
                    return this;
                },
                rowSelected(callback) {
                    _rowSelected = callback;
                }
            });

            function _addColumn(name, title, filterType) {
                const config = {
                    index: _index++,
                    name:  name,
                    title: title
                };

                if (filterType)
                    config.filterType = filterType;

                _columns.push(config);
            }

            function _buildOptions() {
                return optionsBuilder.newOptions()
                    .withOption('ajax',
                    {
                        url:  _url,
                        type: 'POST'
                    })
                    .withDataProp('data')
                    .withOption('processing', true)
                    .withOption('serverSide', true)
                    .withPaginationType('full_numbers')
                    .withDOM('<"html5buttons"B>lTfgitp')
                    .withButtons([
                        { extend: 'copy' },
                        { extend: 'csv' },
                        { extend: 'excel', title: 'ExampleFile' },
                        { extend: 'pdf', title: 'ExampleFile' },
                        {
                            extend: 'print',
                            customize: function(win) {
                                $(win.document.body).addClass('white-bg');
                                $(win.document.body).css('font-size', '10px');

                                $(win.document.body).find('table')
                                    .addClass('compact')
                                    .css('font-size', 'inherit');
                            }
                        }
                    ])
                    .withLightColumnFilter(_columns.reduce((x, y) => {
                        if (y.filterType)
                            x[y.index] = { html: 'input', type: y.filterType, attr: { class: 'form-control' } };
                        return x;
                    },{}))
                    .withOption('rowCallback', _wireClickEvent);
            }

            function _buildColumnOptions() {
                return _columns.reduce((x, y) => {
                    x.push(columnBuilder.newColumn(y.name).withTitle(y.title));
                    return x;
                }, []);
            }

            function _wireClickEvent(nRow, aData) {
                    // Unbind first in order to avoid any duplicate handler
                    //(see https://github.com/l-lin/angular-datatables/issues/87)
                    if (_rowSelected) {
                        $('td', nRow).unbind('click');
                        $('td', nRow).bind('click', () => _rowSelected(aData));
                        return nRow;
                    }
                }
        }
    });

    const TableController = Controller.extend({
        constructor() {
            this.extend({
                initialize() {
                    this.table = new TableManager(this.io);
                    if (this.configure)
                        this.configure(this.table);
                }
            });
        }
    });

    eval(this.exports);

};
