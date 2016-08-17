$(function () {
    $.ajaxSetup({
        traditional: true
    });

    var eventId = $('#eventId').val();
    var eventTypeId = $('#eventTypeId').val();
    var eventName;
    var oneDate;
    var fromDate;
    var toDate;
    var pickedTime;
    var basedOn;
    var difference;
    var restrictionCount = 2;
    var exclusionCount = 2;
    var restrictArray = [];
    var excludeArray = [];

    var time24 = $('#time').val();

    $('#single-date').hide();
    $('#time-pick').hide();
    $('#basedon-difference').hide();
    $('#recurring').hide();
    $('#from-to').hide();

    $('.recurring-update').show();
    $('.single-date-update').show();
    $('.time-pick-update').show();
    $('.basedon-difference-update').show();
    $('.from-to-update').show();

    $('input:radio[name="isOneTime"]').change(function () {
        if ($(this).val() === 'recurring') {
            $('#recurring').show();
            $('#single-date').hide();
        } else {
            $('#recurring').hide();
            $('#single-date').show();
        }
    });

    $('input:radio[name="ifFixed"]').change(function () {
        if ($(this).val() === 'fixed') {
            $('#time-pick').show();
            $('#basedon-difference').hide();
        } else {
            $('#time-pick').hide();
            $('#basedon-difference').show();
        }
    });

    $('#btn-fromTo').click(function () {
        $('#from-to').show();
    });

    $('#clear').click(function () {
        $('#from').val('');
        $('#to').val('');
        $('#from-to').hide();
    });

    $('#add-restriction').click(function () {
        addRestriction();
    });

    $('#add-exclusion').click(function () {
        addExclusion();
    });

    //$('#enter').click(function () {
    //    eventName = $('#event').val();

    //    if ($('input[name=isOneTime]:checked').val() === 'recurring') {
    //        fromDate = $('#from').val();
    //        toDate = $('#to').val();
    //        for (var i = 1; i < restrictionCount; i++) {
    //            restrictArray.push(myRestrict(i));
    //        }
    //        for (var b = 1; b < exclusionCount; b++) {
    //            excludeArray.push(myExclude(b));
    //        }
    //        endDateValidator();
    //    } else {
    //        oneDate = $('#date').val();
    //    }

    //    if ($('input[name=ifFixed]:checked').val() === 'fixed') {
    //        pickedTime = $('#timepicker').val();
    //        pickedTime = pickedTime.replace(' ', '');
    //        pickedTime = pickedTime.replace(' ', '');
    //        difference = 0;
    //        //basedOn = $('#based-on-select').val();
    //        $('.beforeAfterRdo').prop('checked', false);
    //    } else if ($('input[name=ifFixed]:checked').val() === "not-fixed") {
    //        basedOn = $('#based-on-select').val();
    //        if ($('input[name=beforeAfter]:checked').val() === 'before') {
    //            difference = -$('#minutes').val();
    //        } else {
    //            difference = $('#minutes').val();
    //        }
    //    }

    //    if (difference === '') {
    //        difference = 0;
    //    }

    //    if ($('input[name=isOneTime]:checked').val() === 'once') {
    //        $.post('/home/submitEvent', { eventId: eventId, eventName: eventName, date: oneDate, time: pickedTime, basedOn: basedOn, timedifference: difference }, function () {

    //            window.location = "/home/index/";
    //        });
    //    }
    //    if ($('input[name=isOneTime]:checked').val() === 'recurring') {
    //        //console.log(restrictArray);
    //        $.post('/home/SubmitEventType', { eventTypeId: eventTypeId, eventName: eventName, startDate: fromDate, endDate: toDate, time: pickedTime, BasedOn: basedOn, timeDifference: difference, restrictions: restrictArray, exclusions: excludeArray }, function () {

    //            window.location = "/home/index/";
    //        });
    //    }
    //    eventName = '',
    //    oneDate = '',
    //    fromDate = '',
    //    toDate = '',
    //    pickedTime = '',
    //    basedOn = '',
    //    difference = '',
    //    restrictionCount = 2,
    //    exclusionCount = 2,
    //    excludeArray = [];
    //    restrictArray = [];
    //});


    $('#enter').click(function () {
        eventName = $('#event').val();

        if ($('input[name=isOneTime]:checked').val() === 'recurring') {
            fromDate = $('#from').val();
            toDate = $('#to').val();
            for (var i = 1; i < restrictionCount; i++) {
                restrictArray.push(myRestrict(i));
            }
            for (var b = 1; b < exclusionCount; b++) {
                excludeArray.push(myExclude(b));
            }
            endDateValidator();
        } else {
            oneDate = $('#date').val();
        }

        if ($('input[name=ifFixed]:checked').val() === 'fixed') {
            pickedTime = $('#timepicker').val();
            pickedTime = pickedTime.replace(' ', '');
            pickedTime = pickedTime.replace(' ', '');
            difference = 0;
            //basedOn = $('#based-on-select').val();
            $('.beforeAfterRdo').prop('checked', false);
        } else if ($('input[name=ifFixed]:checked').val() === "not-fixed") {
            basedOn = $('#based-on-select').val();
            if ($('input[name=beforeAfter]:checked').val() === 'before') {
                difference = -$('#minutes').val();
            } else {
                difference = $('#minutes').val();
            }
        }

        if (difference === '') {
            difference = 0;
        }

        if ($('input[name=isOneTime]:checked').val() === 'recurring') {
            if (eventName === '' && pickedTime === '' && basedOn === '') {
                alert('You must select or enter a "name" and a "time" for this event');
            } else if (eventName === '') {
                alert('You must select or enter a "name" for this event');
            } else if (pickedTime === '' && basedOn === '') {
                alert('You must select or enter a "time" for this event');
            } else {
                $.post('/home/SubmitEventType', { eventTypeId: eventTypeId, eventName: eventName, startDate: fromDate, endDate: toDate, time: pickedTime, BasedOn: basedOn, timeDifference: difference, restrictions: restrictArray, exclusions: excludeArray }, function () {
                    abc('#enter');
                    window.location = "/home/index/";
                });
            }

        }
        if ($('input[name=isOneTime]:checked').val() === 'once') {
            if (eventName === '') {
                alert('You must select or enter a "name" for this event');
            } else if (oneDate === '') {
                alert('You must select or enter a "date" for this event');
            } else if (pickedTime === '' && basedOn === '') {
                alert('You must select or enter a "time" for this event');
            }
            $.post('/home/submitEvent', { eventId: eventId, eventName: eventName, date: oneDate, time: pickedTime, basedOn: basedOn, timedifference: difference }, function () {

                window.location = "/home/index/";
            });
        }
        eventName = '',
        oneDate = '',
        fromDate = '',
        toDate = '',
        pickedTime = '',
        basedOn = '',
        difference = '',
        restrictionCount = 2,
        exclusionCount = 2,
        excludeArray = [];
        restrictArray = [];
    });


    //restrictions update preload
    for (var h = 2; h <= $('#restrictions-list').val() ; h++) {
        $('#restriction' + h).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        restrictionCount++;
    }
    //exclusions update preload
    for (var j = 2; j <= $('#exclusions-list').val() ; j++) {
        $('#exclusion' + j).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        exclusionCount++;
    }

    //functions
    function addRestriction() {
        $('#more-restrictions').append('<h4>Only when it falls out on:</h4>' +
           '<div style="text-align: left; width: 320px; margin-left: 173px">' +
           '<select name="restriction' + restrictionCount + '[]" multiple id="restriction' + restrictionCount + '">' +
                '<option value="Saturday"@Model.CheckIfSelectedRest("Saturday", ' + restrictionCount + ')>Shabbos</option>' +
                '<option value="YomTov" @Model.CheckIfSelectedRest("YomTov", ' + restrictionCount + ')>Yom Tov</option>' +
                '<option value="RoshChodesh" @Model.CheckIfSelectedRest("RoshChodesh", ' + restrictionCount + ')>Rosh Chodesh</option>' +
                '<option value="Taanis" @Model.CheckIfSelectedRest("Taanis", ' + restrictionCount + ')>Taanis</option>' +
                '<option value="Sunday" @Model.CheckIfSelectedRest("Sunday", ' + restrictionCount + ')>Sunday</option>' +
                '<option value="Monday" @Model.CheckIfSelectedRest("Monday", ' + restrictionCount + ')>Monday</option>' +
                '<option value="Tuesday" @Model.CheckIfSelectedRest("Tuesday", ' + restrictionCount + ')>Tuesday</option>' +
                '<option value="Wednesday" @Model.CheckIfSelectedRest("Wednesday", ' + restrictionCount + ')>Wednesday</option>' +
                '<option value="Thursday" @Model.CheckIfSelectedRest("Thursday", ' + restrictionCount + ')>Thursday</option>' +
                '<option value="Friday" @Model.CheckIfSelectedRest("Friday", ' + restrictionCount + ')>Friday</option>' +
           '</select>' +
           '</div>');

        $('#restriction' + restrictionCount).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        restrictionCount++;
    }



    function addExclusion() {
        $('#more-exclusions').append('<h4 style="margin-top: 20px">and only if also falls out on:</h4>' +
           '<div style="text-align: left; width: 320px; margin-left: 173px">' +
           '<select name="exclusion' + exclusionCount + '[]" multiple id="exclusion' + exclusionCount + '">' +
           '<option value="Saturday"@Model.CheckIfSelectedExc("Saturday", ' + exclusionCount + ')>Shabbos</option>' +
                '<option value="YomTov" @Model.CheckIfSelectedExc("YomTov", ' + exclusionCount + ')>Yom Tov</option>' +
                '<option value="RoshChodesh" @Model.CheckIfSelectedExc("RoshChodesh", ' + exclusionCount + ')>Rosh Chodesh</option>' +
                '<option value="Taanis" @Model.CheckIfSelectedExc("Taanis", ' + exclusionCount + ')>Taanis</option>' +
                '<option value="Sunday" @Model.CheckIfSelectedExc("Sunday", ' + exclusionCount + ')>Sunday</option>' +
                '<option value="Monday" @Model.CheckIfSelectedExc("Monday", ' + exclusionCount + ')>Monday</option>' +
                '<option value="Tuesday" @Model.CheckIfSelectedExc("Tuesday", ' + exclusionCount + ')>Tuesday</option>' +
                '<option value="Wednesday" @Model.CheckIfSelectedExc("Wednesday", ' + exclusionCount + ')>Wednesday</option>' +
                '<option value="Thursday" @Model.CheckIfSelectedExc("Thursday", ' + exclusionCount + ')>Thursday</option>' +
                '<option value="Friday" @Model.CheckIfSelectedExc("Friday", ' + exclusionCount + ')>Friday</option>' +
           '</select>' +
           '</div>');

        $('#exclusion' + exclusionCount).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        exclusionCount++;
    }



    function myRestrict(index) {
        var restrictSubArr = [];
        $('#restriction' + index + ' :selected').each(function (i, selected) {
            restrictSubArr[i] = $(selected).val();
        });
        return restrictSubArr.join();
    }

    function myExclude(index) {
        var excludeSubArr = [];
        $('#exclusion' + index + ' :selected').each(function (i, selected) {
            excludeSubArr[i] = $(selected).val();
        });
        return excludeSubArr.join();
    }

    function endDateValidator() {
        if (new Date($('#from').val()) > new Date($('#to').val())) {
            alert("'Oops.. End Date' can not be before 'Start Date'");
        }
    }


    //multiselect dropdowns
    $('#restriction1').multiselect({
        columns: 1,
        placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
    });

    $('#exclusion1').multiselect({
        columns: 1,
        placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
    });

    $('#exceptions').multiselect({
        columns: 1,
        placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
    });



    //timepicker
    var options = {
        now: time24, //hh:mm 24 hour format only, defaults to current time
        twentyFour: false,  //Display 24 hour format, defaults to false
        upArrow: 'wickedpicker__controls__control-up',  //The up arrow class selector to use, for custom CSS
        downArrow: 'wickedpicker__controls__control-down', //The down arrow class selector to use, for custom CSS
        close: 'wickedpicker__close', //The close class selector to use, for custom CSS
        hoverState: 'hover-state', //The hover state class to use, for custom CSS
        title: '', //The Wickedpicker's title,
        showSeconds: false, //Whether or not to show seconds,
        secondsInterval: 1, //Change interval for seconds, defaults to 1,
        minutesInterval: 1, //Change interval for minutes, defaults to 1
        beforeShow: null, //A function to be called before the Wickedpicker is shown
        show: null, //A function to be called when the Wickedpicker is shown
        clearable: false, //Make the picker's input clearable (has clickable "x")
    };
    $('.timepicker').wickedpicker(options);

    //datepickers
    $(".datepicker").datepicker({
        minDate: new Date($.now())
    });

    function abc(this1) {
        //alert('asdasd');
        this1.disabled = true;
        this1.innerHTML = 'Processing…';

    }

});