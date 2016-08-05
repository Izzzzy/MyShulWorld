$(document).ready(function () {
    //var eventsAjax;
    //$.get("/home/getevents?start=2016-08-01&end=2016-09-01", function (result) {
    //    eventsAjax = result.events;
    //});
    

    $("#calendar").fullCalendar({
        header: {
            left: "prev,next today",
            center: "title",
            right: "month,agendaWeek,agendaDay"
        },
        editable: false,
        fixedWeekCount: false,
        timezone: false,
       // events: {
        
       //     url: "https://www.hebcal.com/hebcal/?cfg=fc&v=1&i=off&maj=on&min=on&nx=on&mf=on&ss=on&mod=on&lg=s&s=on",

       //     cache: true
       //}
        eventSources: [

            {
                url: "https://www.hebcal.com/hebcal/?cfg=fc&v=1&i=off&maj=on&min=on&nx=on&mf=on&ss=on&mod=on&lg=s&s=on",
                cache: true
            },

            {
                url: "/home/GetEvents"
                //events: eventsAjax
                //textColor: 'black'
            }

        ]
    });
    //$("#calendar").fullCalendar({
    //    events: eventsAjax
    //    //    [
    //    //{
    //    //    eventsAjax
    //    //    }
    //    //]
    //});

    $("body").keydown(function (e) {
        if (e.keyCode === 37) {
            $('#calendar').fullCalendar('prev');
        } else if (e.keyCode === 39) {
            $('#calendar').fullCalendar('next');
        }
    });
});















