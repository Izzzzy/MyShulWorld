$(function () {


    var eventChosen;
    var oneDate;
    var fromDate;
    var toDate;
    var pickedTime;
    var basedOn;
    var difference;
    var beforeAfter;
    var recurrTypes;
    var recurrDays;
    var recurrExceptions;


    $('#single-date').hide();
    $('#time-pick').hide();
    $('#based-on').hide();
    $('#minutes-difference').hide();
    $('#recurring').hide();
    $('#select-list').hide();
    $('#from-to').hide();


    $('#test').click(function () {
        //alert($('#from').val());
        endDateValidator();
    });
    //$('#test2').click(function () {
    //    $('.uncheck').prop('checked', false);
    //});


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

    $('#enter').click(function () {


        eventChosen = $('#event').val();

        if ($('input[name=isOneTime]:checked').val() === 'recurring') {
            recurrTypes = myTypes();
            recurrDays = myDays();
            recurrExceptions = myExceptions();
            fromDate = $('#from').val();
            toDate = $('#to').val();
            endDateValidator();

        } else {
            oneDate = $('#date').val();
        }

        if ($('input[name=ifFixed]:checked').val() === 'fixed') {
            pickedTime = $('#timepicker').val();
            $('.beforeAfterRdo').prop('checked', false);
        } else if ($('input[name=ifFixed]:checked').val() === "Based on another z'man") {
            basedOn = $('#based-on-select').val();
            difference = $('#minutes').val();
            beforeAfter = $('input[name=beforeAfter]:checked').val();
        }


    });

    //functions

    function myTypes() {
        var bar = [];
        $('#types :selected').each(function (i, selected) {
            bar[i] = $(selected).text();
        });
        return bar;
    }

    function myDays() {
        var foo = [];
        $('#days :selected').each(function (i, selected) {
            foo[i] = $(selected).text();
        });
        return foo;
    }

    function myExceptions() {
        var hello = [];
        $('#days :selected').each(function (i, selected) {
            hello[i] = $(selected).text();
        });
        return hello;
    }

    function endDateValidator() {
        if (new Date($('#from').val()) > new Date($('#to').val())) {
            alert("'End Date' can not be before 'Start Date'");
        }
    }

    //multiselect dropdowns

    $('#types').multiselect({
        columns: 1,
        placeholder: 'e.g. Rosh Chodesh, Taanis etc.'
    });

    $('#days').multiselect({
        columns: 1,
        placeholder: 'e.g. Sunday, Monday etc.'
    });

    $('#exceptions').multiselect({
        columns: 1,
        placeholder: 'e.g. If it falls out on Shabbos, Rosh Chodesh etc.'
    });



    //timepicker
    $('.timepicker').wickedpicker();

    //datepickers
    $(".datepicker").datepicker({
        minDate: new Date($.now())
        //minDate: -20, maxDate: "+1M +10D"
    });





});


