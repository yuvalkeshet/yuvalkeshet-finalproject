
!function ($) {
    "use strict";

    var CalendarApp = function () {
        this.$body = $("body")
        this.$modal = $('#event-modal'),
            this.$event = ('#external-events div.external-event'),
            this.$calendar = $('#calendar'),
            this.$saveCategoryBtn = $('.save-category'),
            this.$categoryForm = $('#add-category form'),
            this.$extEvents = $('#external-events'),
            this.$calendarObj = null
    };

    var shiftTypes;



    function getNextDayOfWeek(date, dayOfWeek) {
        // Code to check that date and dayOfWeek are valid left as an exercise ;)

        var resultDate = new Date(date.getTime());
        var daysUntil = (7 + dayOfWeek - date.getDay()) % 7;
        if (daysUntil == 0)
            daysUntil = 7;
        resultDate.setDate(date.getDate() + daysUntil);

        return resultDate;
    }


    /* on drop */
    CalendarApp.prototype.onDrop = function (eventObj, date) {
        var $this = this;
        // retrieve the dropped element's stored Event Object
        var originalEventObject = eventObj.data('eventObject');
        var $categoryClass = eventObj.attr('data-class');
        // we need to copy it, so that multiple events don't have a reference to the same object
        var copiedEventObject = $.extend({}, originalEventObject);
        // assign it the date that was reported
        copiedEventObject.start = date;
        if ($categoryClass)
            copiedEventObject['className'] = [$categoryClass];
        // render the event on the calendar
        $this.$calendar.fullCalendar('renderEvent', copiedEventObject, true);
        // is the "remove after drop" checkbox checked?
        if ($('#drop-remove').is(':checked')) {
            // if so, remove the element from the "Draggable Events" list
            eventObj.remove();
        }
    },
        /* on click on event */
        CalendarApp.prototype.onEventClick = function (calEvent, jsEvent, view) {

            var fromDate = getNextDayOfWeek(new Date(), 0);
            var tempDate = fromDate;
            var toDate = new Date();
            toDate.setDate(tempDate.getDate() + 6);
            //fromDate.setHours(0, 0, 0, 0)
            //fromDate.setHours(0, 0, 0, 0)



            var $this = this;
            var form = $("<form></form>");
            form.append("<label>Shift</label>");
            form.append("<div class='input-group'><input class='form-control' disabled type=text value='" + calEvent.title +
                "' />&nbsp<input class='form-control' disabled type=text value='"
                + calEvent.shiftType + " (" + calEvent.duration + ")' /></div>");
            $this.$modal.modal({
                backdrop: 'static'
            });

            if (calEvent.start._d < fromDate ||
                calEvent.start._d > toDate) {


                $this.$modal.find('.delete-event').show().end().find('.save-event').hide().end().find('.modal-body').empty().prepend(form).end().find('.delete-event').hide();
            }
            else {
                $this.$modal.find('.delete-event').show().end().find('.save-event').hide().end().find('.modal-body').empty().prepend(form).end().find('.delete-event').unbind('click').on("click", function () {
                    $this.$calendarObj.fullCalendar('removeEvents', function (ev) {
                        return (ev._id == calEvent._id);
                    });


                    $this.$modal.modal('hide');
                });
            }
            $this.$modal.find('form').on('submit', function () {
                calEvent.title = form.find("input[type=text]").val();
                $this.$calendarObj.fullCalendar('updateEvent', calEvent);
                $this.$modal.modal('hide');
                return false;
            });
        },
        /* on select */
        CalendarApp.prototype.onSelect = function (start, end, allDay, name) {

            var fromDate = getNextDayOfWeek(new Date(), 0);
            var tempDate = fromDate;
            var toDate = new Date();
            toDate.setDate(tempDate.getDate() + 6);
            //fromDate.setHours(0, 0, 0, 0)
            //fromDate.setHours(0, 0, 0, 0)

            if (start._d < fromDate ||
                start._d > toDate) {
                swal("Something went wrong", "You may post shifts for the nex week only\n("
                    + fromDate.toDateString() + " - " + toDate.toDateString() + ")", "error");
                return;
            }

            var $this = this;
            $this.$modal.modal({
                backdrop: 'static'
            });
            var form = $("<form></form>");
            form.append("<div class='row'></div>");

            form.find(".row")
                .append("<div class='col-md-6'><div class='form-group'><label class='control-label'>Employee</label><input class='form-control' disabled type='text' value='" + name + "' name='title'/></div></div>")
                .append("<div class='col-md-6'><div class='form-group'><label class='control-label'>Shift Type</label><select class='form-control' name='category'></select></div></div>");

            for (var i = 0; i < shiftTypes.length; i++) {
                form.find(".row").find("select[name='category']")
                    .append("<option value='bg-dark'>" + shiftTypes[i].ShiftName + "</option>");
            }

            $this.$modal.find('.delete-event').hide().end().find('.save-event').show().end().find('.modal-body').empty().prepend(form).end().find('.save-event').unbind('click').on("click", function () {
                form.submit();
            });
            $this.$modal.find('form').on('submit', function () {
                var title = form.find("input[name='title']").val();
                var beginning = form.find("input[name='beginning']").val();
                var ending = form.find("input[name='ending']").val();
                var categoryClass = form.find("select[name='category'] option:checked").val();
                var shiftType = form.find("select[name='category'] option:checked").html();
                var s = shiftTypes.find(function (shift) {
                    return shift.ShiftName === shiftType;
                });

                if (title !== null && title.length != 0) {
                    $this.$calendarObj.fullCalendar('renderEvent', {
                        title: title,
                        start: start,
                        end: end,
                        allDay: false,
                        className: categoryClass,
                        shiftType: shiftType,
                        shiftCode: s.ShiftType,
                        duration: s.StartTime + " - " + s.EndTime
                    }, true);


                    $this.$modal.modal('hide');

                }
                else {
                    alert('You have to give a title to your event');
                }
                return false;

            });
            $this.$calendarObj.fullCalendar('unselect');
        },
        CalendarApp.prototype.enableDrag = function () {
            //init events
            $(this.$event).each(function () {
                // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
                // it doesn't need to have a start or end
                var eventObject = {
                    title: $.trim($(this).text()) // use the element's text as the event title
                };
                // store the Event Object in the DOM element so we can get to it later
                $(this).data('eventObject', eventObject);
                // make the event draggable using jQuery UI
                $(this).draggable({
                    zIndex: 999,
                    revert: true,      // will cause the event to go back to its
                    revertDuration: 0  //  original position after the drag
                });
            });
        }
    /* Initializing */
    CalendarApp.prototype.init = function () {
        var Info = JSON.parse(localStorage['user_data']);
        var myEvents = [];
        var myName;
        var $this = this;
        $("#btnPostShifts").on("click", function () {
            var events = $this.$calendarObj.fullCalendar('clientEvents');
            var shifts = [];
            for (var i = 0; i < events.length; i++) {
                var s = {
                    Email: Info.Email,
                    ShiftType: events[i].shiftCode,
                    ShiftDate: events[i].start._d.toDateString()
                };
                shifts.push(s);
            }

            //if (shifts.length == 0) {
            //    swal("Something went wrong", "You have to post at least 1 shift", "error");
            //    return;
            //}

            $.ajax({
                type: "POST",
                url: "api/constraints/?email=" + Info.Email,
                data: JSON.stringify(shifts),
                cache: false,
                contentType: "application/json",
                dataType: "json",
                async: true,
                success: function (result) {
                    swal("Great!", "Shifts have been posted, and are pending approval", "success");
                },
                error: function (err) {
                    swal("Something went wrong", err.responseJSON.ExceptionMessage, "error");
                }
            });

        });


        //if (Info.isManager)
        //    $('#editBtn').show();

        //if (Info.isManager)
        //    $('#editBtn').show();
        $.ajax({
            type: "GET",
            url: "api/employee/?email=" + Info.Email,
            data: "",
            cache: false,
            contentType: "application/json",
            dataType: "json",
            async: true,
            success: function (result) {
                myName = result.Fname + " " + result.Lname;



            },
            error: function (err) {
                swal("Something went wrong", err.responseJSON.ExceptionMessage, "error");
            }
        }).done(function () {
            $.ajax({
                type: "GET",
                url: "api/shift",
                data: "",
                cache: false,
                contentType: "application/json",
                dataType: "json",
                async: true,
                success: function (result) {

                    shiftTypes = result;

                },
                error: function (err) {
                    swal("Something went wrong", err.responseJSON.ExceptionMessage, "error");
                }
            }).done(function () {
                $.ajax({
                    type: "GET",
                    url: "api/constraints/?email=" + Info.Email,
                    data: "",
                    cache: false,
                    contentType: "application/json",
                    dataType: "json",
                    async: true,
                    success: function (shifts) {
                        for (var i = 0; i < shifts.length; i++) {
                            var s = shiftTypes.find(function (shift) {
                                return shift.ShiftType === shifts[i].ShiftType;
                            });
                            var sJson = {
                                title: myName,
                                start: shifts[i].ShiftDate,
                                className: 'bg-primary',
                                shiftType: s.ShiftName,
                                shiftCode: shifts[i].ShiftType,
                                duration: s.StartTime + " - " + s.EndTime
                            };

                            myEvents.push(sJson);
                        }




                    },
                    error: function (err) {
                        swal("Something went wrong", err.responseJSON.ExceptionMessage, "error");
                    }
                }).done(function () {
                    $this.enableDrag();
                    /*  Initialize the calendar  */
                    var date = new Date();
                    var d = date.getDate();
                    var m = date.getMonth();
                    var y = date.getFullYear();
                    var form = '';
                    var today = new Date($.now());


                    $this.$calendarObj = $this.$calendar.fullCalendar({
                        slotDuration: '00:15:00', /* If we want to split day time each 15minutes */
                        minTime: '06:00:00',
                        maxTime: '24:00:00',
                        defaultView: 'month',
                        handleWindowResize: true,
                        height: $(window).height() - 200,
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month'
                        },
                        eventTextColor: '#fff',
                        displayEventTime: false,
                        events: myEvents,
                        editable: true,
                        droppable: true, // this allows things to be dropped onto the calendar !!!
                        eventLimit: true, // allow "more" link when too many events
                        selectable: true,
                        drop: function (date) { $this.onDrop($(this), date); },
                        select: function (start, end, allDay, name) { $this.onSelect(start, end, allDay, myName); },
                        eventClick: function (calEvent, jsEvent, view) { $this.onEventClick(calEvent, jsEvent, view); }


                    });
                });
            });
        });








        //on new event
        this.$saveCategoryBtn.on('click', function () {
            var categoryName = $this.$categoryForm.find("input[name='category-name']").val();
            var categoryColor = $this.$categoryForm.find("select[name='category-color']").val();
            if (categoryName !== null && categoryName.length != 0) {
                $this.$extEvents.append('<div class="external-event bg-' + categoryColor + '" data-class="bg-' + categoryColor + '" style="position: relative;"><i class="fa fa-move"></i>' + categoryName + '</div>')
                $this.enableDrag();
            }

        });
    },

        //init CalendarApp
        $.CalendarApp = new CalendarApp, $.CalendarApp.Constructor = CalendarApp

}(window.jQuery),

    //initializing CalendarApp
    function ($) {
        "use strict";
        $.CalendarApp.init()
    }(window.jQuery);
