$(function () {
    $.ajaxSetup({
        traditional: true
    });

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

    $('#single-date').hide();
    $('#time-pick').hide();
    $('#based-on').hide();
    $('#minutes-difference').hide();
    $('#recurring').hide();
    $('#select-list').hide();
    $('#from-to').hide();


    $('#test').click(function () {

        console.log(excludeArray);
        //endDateValidator();
    });

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
            $('#based-on').hide();
            $('#minutes-difference').hide();

        } else {
            $('#time-pick').hide();
            $('#based-on').show();
            $('#minutes-difference').show();
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
        $('#more-restrictions').append('<h4>Only when it falls out on:</h4>' +
            '<div style="text-align: left; width: 320px; margin-left: 173px">' +
            '<select name="restriction' + restrictionCount + '[]" multiple id="restriction' + restrictionCount + '">' +
            '<option value="Sunday">Sunday</option>' +
            '<option value="Monday">Monday</option>' +
            '<option value="Tuesday">Tuesday</option>' +
            '<option value="Wednesday">Wednesday</option>' +
            '<option value="Thursday">Thursday</option>' +
            '<option value="Friday">Friday</option>' +
            '<option value="Shabbos">Shabbos</option>' +
            '<option value="YomTov">Yom Tov</option>' +
            '<option value="RoshChodesh">Rosh Chodesh</option>' +
            '<option value="Taanis">Taanis</option>' +
            '</select>' +
            '</div>');

        $('#restriction' + restrictionCount).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        restrictionCount++;
    });

    $('#add-exclusion').click(function () {

        $('#more-exclusions').append('<h4 style="margin-top: 20px">and only if also falls out on:</h4>' +
            '<div style="text-align: left; width: 320px; margin-left: 173px">' +
            '<select name="exclusion' + exclusionCount + '[]" multiple id="exclusion' + exclusionCount + '">' +
            '<option value="Sunday">Sunday</option>' +
            '<option value="Monday">Monday</option>' +
            '<option value="Tuesday">Tuesday</option>' +
            '<option value="Wednesday">Wednesday</option>' +
            '<option value="Thursday">Thursday</option>' +
            '<option value="Friday">Friday</option>' +
            '<option value="Shabbos">Shabbos</option>' +
            '<option value="YomTov">Yom Tov</option>' +
            '<option value="RoshChodesh">Rosh Chodesh</option>' +
            '<option value="Taanis">Taanis</option>' +
            '</select>' +
            '</div>');

        $('#exclusion' + exclusionCount).multiselect({
            columns: 1,
            placeholder: 'e.g. Sunday, Shabbos, Rosh Chodesh etc.'
        });
        exclusionCount++;
    });

    $('#enter').click(function () {
        eventName = $('#event').val();

        if ($('input[name=isOneTime]:checked').val() === 'recurring') {
            fromDate = $('#from').val();
            toDate = $('#to').val();
            for (var i = 1; i < restrictionCount; i++) {
                restrictArray.push(myRestrict(i));
            }
            for (var j = 1; j < exclusionCount; j++) {
                excludeArray.push(myExclude(j));
            }
            endDateValidator();
        } else {
            oneDate = $('#date').val();
        }

        if ($('input[name=ifFixed]:checked').val() === 'fixed') {
            pickedTime = $('#timepicker').val();
            difference = 0;
            basedOn = $('#based-on-select').val();
            $('.beforeAfterRdo').prop('checked', false);
        } else if ($('input[name=ifFixed]:checked').val() === "not-fixed") {
            basedOn = $('#based-on-select').val();
            if ($('input[name=beforeAfter]:checked').val() === 'before') {
                difference = -$('#minutes').val();
            } else {
                difference = $('#minutes').val();
            }
        }
        if ($('input[name=isOneTime]:checked').val() === 'once') {
            $.post('/home/submitEvent', { eventName: eventName, date: oneDate, time: pickedTime, basedOn: basedOn, timedifference: difference }, function () {
                
                window.location="/home/index/";
            });
        }
        if ($('input[name=isOneTime]:checked').val() === 'recurring') {
            console.log(restrictArray);
            $.post('/home/SubmitEventType', { eventName: eventName, startDate: fromDate, endDate: toDate, time: pickedTime, BasedOn: basedOn, timeDifference: difference, restrictions: restrictArray, exclusions: excludeArray }, function () {
            
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
    $('.timepicker').wickedpicker();

    //datepickers
    $(".datepicker").datepicker({
        minDate: new Date($.now())
        //minDate: -20, maxDate: "+1M +10D"
    });

});


