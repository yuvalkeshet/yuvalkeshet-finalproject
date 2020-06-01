function posSuccess(data) {
    $.each(data, function (key, entry) {
        $("#posSelEdit").append($('<option></option>').attr('value', entry.PosName).text(entry.PosName));
    });
}

function posError(err) {
    alert("Error inserting employee");
}

function posInsertSuccess(data) {
    $.each(data, function (key, entry) {
        $("#posSelInsert").append($('<option></option>').attr('value', entry.PosName).text(entry.PosName));
    });
}

function posInsertError(err) {
    alert("Error inserting employee - " + err);
}



$(function () {


  



        $("#jsGrid").jsGrid({
            height: "100%",
            width: "100%",
            filtering: false,
            editing: true,
            inserting: true,
            sorting: true,
            paging: true,
            autoload: true,
            pageSize: 15,
            pageButtonCount: 5,
            deleteConfirm: "Do you really want to delete the employee?",
            controller: db,
            onItemInserting: function (args) {
                if (args.item.Email == "" || args.item.Email == null) {
                    alert("Email is required");
                    args.cancel = true;
                }

                else if (args.item.IsManager === true) {
                    //args.cancel = true;
                    r = confirm("you are going to make this Employee a manager. Managers can manage Other employees and users and have special authorizations.\n\n Are you sure you want to proceed?");
                    if (r == false)
                        args.cancel = true;
                }
            },
            onItemEditing: function (args) {
                // cancel editing of the row of item with field 'ID' = 0
                //if (args.item.Fname == "" || args.item.Fname == null) {
                //    alert("asdasdasd");
                //    args.cancel = true;
                //}
            },
            onItemUpdating: function (args) {
                // cancel update of the item with empty 'name' field
                if (args.item.IsManager && !args.previousItem.IsManager) {
                    //args.cancel = true;
                    r = confirm("you are goung to make this Employee a manager. Managers can manage Other employees and users and have special authorizations.\n\n Are you sure you want to proceed?");
                    if (r == false)
                        args.cancel = true;
                }
            },
            onOptionChanged: function (args) {
                if (args.option == "inserting") {
                    ajaxCall("GET", "../api/department/getPositions/?depName=" +
                        $('#selDepInsert').val(), "", posInsertSuccess, posInsertError, async = false);
                    //args.data[7].StartDate = new Date();
                }
            },
            //onDataLoaded: function (args) {
            //    //args.grid.fields[10].renderTemplate();
            //},

            //invalidNotify: function (args) {
            //    if(args.grid.fields)
            //},
            //invalidMessage: "Invalid data entered!",

            //onItemInvalid: function (args) {
            //    // prints [{ field: "Name", message: "Enter client name" }]
            //    console.log(args.errors);
            //},

          
            fields: [
                { name: "Email", type: "text", width: 200, editing: false },
                { name: "Fname", type: "text", title: "First Name", width: 150 },
                { name: "Lname", type: "text", title: "Last Name", width: 150 },
                { name: "Salary", type: "number", width: 100 },
                { name: "Seniority", type: "number", width: 100 },
                { name: "PhoneNo", type: "text", title: "Phone No.", width: 150 },
                { name: "Address", type: "text", width: 200 },
                {
                    name: "StartDate", type: "date", title: "Start Date", width: 150, myCustomProperty: "bar", editing: false, inserting: false
                    //insertTemplate: function (value, item) {

                    //    return $("<input>").attr("type", "date").attr("value", Date.now());
                    //    //var $result = jsGrid.fields.text.prototype.insertTemplate.call(this, value); // original input

                    //    //$result.val(new Date());

                    //    //return $result;
                    //}
                },


                { name: "BirthDate", type: "date", title: "Birth Date", width: 150, myCustomProperty: "bar", editing: false },
                {
                    name: "Gender", type: "select", items: [
                        { Name: "Male", Id: "Male" },
                        { Name: "Female", Id: "Female" }],
                    valueField: "Id", textField: "Name"
                },
                { name: "IsMarried", type: "checkbox", title: "Is Married", sorting: false },
                {
                    name: "Position", 
                    editTemplate: function (value, item) {

                        //var $editControl = jsGrid.fields.select.prototype.editTemplate.call(this, value);
                        //$editControl.attr("id", "posSelEdit");

                        var str = "";
                        $.ajax({
                            type: "GET",
                            url: "../api/department/getPositions/?depName=" + item.Department,
                            contentType: "application/json",
                            dataType: "json",
                            async: false,
                            success: function (data) {

                                str = "<select id='posSelEdit'>";
                                $.each(data, function (key, entry) {
                                    if (entry.PosName.toLowerCase() == item.Position.toLowerCase())
                                        //$editControl.append($("<option></option>")
                                        //    .attr("value", entry.PosName)
                                        //    .text(entry.PosName)); 
                                        str += "<option value='" + entry.PosName + "' selected>" + entry.PosName + "</option>";
                                    else
                                        //$editControl.append($("<option></option>")
                                        //    .attr("value", entry.PosName)
                                        //    .text(entry.PosName)); 
                                        str += "<option value='" + entry.PosName + "'>" + entry.PosName + "</option>";
                                })

                                str += "</select>";
                            },
                            error: function (err) {
                                alert("Error getting positions");
                            }
                        });

                        return str;

                    },
                    insertTemplate: function (value, item) {

                        return "<select id='posSelInsert'></select>";

                    }
                    //cellRenderer: function (value, item) {
                    //    //this.items = ajaxCall("GET", "../api/department/getPositions/?depName=" + item.Department, "", posSuccess, posError)
                    //    var str = "<input type=select, name=Position";
                    //    $.getJSON("../api/department / getPositions /? depName =" + item.Department, function (data) {
                    //        $.each(data, function (key, entry) {
                    //            this.append($('<option></option>').attr('value', entry.DepName).text(entry.DepName));
                    //        })

                    //    return $("<td>").append($("<input type=select>").)(ajaxCall("GET", "../api/department/getPositions/?depName=" + item.Department, "", posSuccess, posError));
                    //} 
                },
                {
                    name: "Department", type: "select", items: db.departments,
                    valueField: "DepName", textField: "DepName",

                    editTemplate: function (value) {
                        // Retrieve the DOM element (select)
                        // Note: prototype.editTemplate                       

                        var $editControl = jsGrid.fields.select.prototype.editTemplate.call(this, value);

                        // Attach onchange listener !
                        $editControl.change(function () {
                            var selectedValue = $(this).val();

                            $('#posSelEdit')
                                .find('option')
                                .remove()
                                ;

                            ajaxCall("GET", "../api/department/getPositions/?depName=" +
                                selectedValue, "", posSuccess, posError, async = false)

                        });

                        return $editControl;
                    },
                    insertTemplate: function (value, item) {

                        var $insertControl = jsGrid.fields.select.prototype.insertTemplate.call(this, value);
                        $insertControl.attr("id", "selDepInsert");

                        // Attach onchange listener !
                        $insertControl.change(function () {
                            var selectedValue = $(this).val();

                            $('#posSelInsert')
                                .find('option')
                                .remove()
                                ;

                            ajaxCall("GET", "../api/department/getPositions/?depName=" +
                                selectedValue, "", posInsertSuccess, posInsertError, async = false)

                        });

                        return $insertControl;

                    }

                    //cellRenderer: function (value, item) {
                    //    return "<td>aaaaa</td>";
                    //} 
                },
                { name: "IsManager", type: "checkbox", title: "Is Manager", sorting: false },
                //{
                //    name: "Password",

                //},
                //{ name: "RegisterDate", type: "customDateTimeField", width: 100, align: "center" },
                { type: "control" }
            ]
        });

    });