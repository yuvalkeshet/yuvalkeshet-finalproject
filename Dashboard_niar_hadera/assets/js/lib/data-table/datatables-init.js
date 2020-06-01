(function ($) {
    //    "use strict";


    /*  Data Table
    -------------*/

    // $('#bootstrap-data-table').DataTable();

    function getSuccess(employees){

    }

    function getError(err) {
        alert(err.responseJSON.ExceptionMessage);
    }

    $('#bootstrap-data-table').DataTable({
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]]
    });



    $('#bootstrap-data-table-export').DataTable({
        dom: 'lBfrtip',
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });


    //ajaxCall("GET", "../api/employees/registered", "", getSuccess, getError);

    function getUsersError(err) {
        alert("Error getting users - " + err);
    }

    function getUsersSucccess(userdata) {

        $('#row-select').DataTable({
            data: userdata,
            columns: [
                { data: "Email" },
                { data: "Password" },
                { data: "Email" },
                { data: "Age" },
                { data: "Height" },
                { data: "Address" },
                { data: "PhoneNo" },
                {
                    data: "IsActive",
                    render: function (data, type, row, meta) {
                        if (data == true)
                            return "<input type='checkbox' id='"
                                + row.Id + "' value='1' checked='checked' onclick='toggleActive(this)'>";
                        return "<input type='checkbox' id='"
                            + row.Id + "' value='0' onclick='toggleActive(this)'>";
                    }

                },
                {
                    data: "IsPremium",
                    render: function (data, type, row, meta) {

                        if (data == true)
                            return "<input type='checkbox' value='1' checked='checked' disabled>";
                        return "<input type='checkbox' value='0' disabled>";

                    }
                },
                { data: "Image", render: getImg }
            ],
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select class="form-control"><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            }
        });
    }






})(jQuery);