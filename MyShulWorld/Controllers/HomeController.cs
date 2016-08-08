using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zmanim.Data;

namespace MyShulWorld.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult EventEntry()
        {
            return View();
        }

        public ActionResult Index()
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            //gr.PopulateShkiaFor30Days();
            return View();
        }

        public ActionResult GetEvents(string start, string end)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            var es = gr.GetEventsBetweenDates(start, end);
            var nl = es.Select(p => new EventVM { start = p.Start, title = p.Title });

            return Json(nl, JsonRequestBehavior.AllowGet);

            //var r = new List<EventVM>() { new EventVM { title = "birthday", start = "2016-08-21" }, { new EventVM { title = "happy", start = "2016-08-23" } } };
            //return Json(r.ToArray(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult SubmitEventsForYear(string year)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.PopulateEventsForAYear(year, gr.GetAllEventTypes());
            return Redirect("/");
        }

        public ActionResult SubmitDeleteEvent(int eventId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.DeleteEvent(eventId);
            return Redirect("/");
        }

        //public void test()//IEnumerable<string> restrictions)
        //{
        //    var x = 0;
        //}
    

        //public ActionResult SubmitEventEntry(bool recurring, string eventName, DateTime date, bool isFixed, string time, BasedOn basedOn, int timeDifference, string identifier, DateTime startDate, DateTime endDate)
        //{
        //    var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
        //    if (!recurring)
        //    {
        //        var e = new Event
        //        {
        //            Date = date,
        //            EventName = eventName
        //        };
        //        if (isFixed)
        //        {
        //            e.Time = time;
        //        }
        //        else
        //        {
        //            e.Time = gr.GetTimeBasedOnSomething(date, (int?)basedOn, timeDifference);
        //        }
        //        gr.AddEvent(e);
        //    }
        //    else
        //    {
        //        var et = new EventType
        //        {
        //            Name = eventName,
        //            Identifier = identifier,
        //            StartDate = startDate,
        //            EndDate = endDate
        //        };
        //        if (isFixed)
        //        {
        //            et.FixedTime = time;
        //        }
        //        else
        //        {
        //            et.BasedOn = (int?)basedOn;
        //            et.TimeDifference = timeDifference;
        //        }
        //        gr.AddEventType(et);
        //    }
        //    return Redirect("/");
        //}
        [HttpPost]
        public ActionResult SubmitEvent(string eventName, DateTime date, string time, BasedOn basedOn, int timeDifference)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);

            var e = new Event
            {
                EventName = eventName,
                Date = date,
                Time =
                    !String.IsNullOrEmpty(time)
                        ? time
                        : gr.GetTimeBasedOnSomething(date, (int?) basedOn, timeDifference)
            };

            gr.AddEvent(e);

            return Redirect("/");
        }

        public ActionResult SubmitEventType(string eventName, string time, BasedOn basedOn, int timeDifference,IEnumerable<string>restrictions,IEnumerable<string>exclusions)
        { 
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);

            var et = new EventType
            {
                Name = eventName
            };
            if (!String.IsNullOrEmpty(time))
            {
                et.FixedTime = time;
            }
            else
            {
                et.BasedOn = (int?)basedOn;
                et.TimeDifference = timeDifference;
            }
            gr.AddEventType(et,restrictions,exclusions);

            return Redirect("/");
        }


        //public ActionResult Calendar()
        //{
        //    return View();
        //}
    }
}
