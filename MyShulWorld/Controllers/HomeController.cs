using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShulWorld.Models;
using Zmanim.Data;

namespace MyShulWorld.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult DeleteAllEventTypesAndEvents()
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.DeleteAllEventsAndEventTypes();
            return Redirect("/");
        }

        public ActionResult EventEntry(EventType et, Event e, int? eventId,int? eventTypeId)
        {
            var uvm = new UpdatesViewModel();

            if (et.Name != null)
            {
                uvm.Et = et;
            }
            else if (e.EventName != null)
            {
                uvm.E = e;
            }

            else if (eventId != null)
            {
                var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
                uvm.E = gr.GetEventById(eventId);
            }

            else if (eventTypeId != null)
            {
                var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
                uvm.Et = gr.GetEventTypeById(eventTypeId);
            }

            return View(uvm);
        }

        public ActionResult Index()
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            var ivm=new IndexViewModel();
            ivm.AllEventTypes = gr.GetAllEventTypes();
            return View(ivm);
        }

        public ActionResult GetEvents(string start, string end)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            var es = gr.GetEventsBetweenDates(start, end);
            var nl = es.Select(p => new EventVM { start = p.Start, title = p.Title, id = p.Id, url = p.Url });

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

        //if doing this way, need to change the delete functions in repo to take in nullable int
        public ActionResult SubmitDeletion(int? eventId,int?eventTypeId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            if (eventId != null)
            {
               // gr.DeleteEvent(eventId);
            }

            else if (eventTypeId != null)
            {
               // gr.DeleteEventType(eventTypeId);
            }
            
            return Redirect("/");
        }

        public ActionResult SubmitDeleteEvent(int eventId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.DeleteEvent(eventId);
            return Redirect("/");
        }

        public ActionResult SubmitDeleteEventType(int eventTypeId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.DeleteEventType(eventTypeId);
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
        public ActionResult SubmitEvent(string eventName, DateTime date, string time, BasedOn basedOn, int timeDifference,int? eventId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            var e = new Event
            {
                EventName = eventName,
                Date = date,
                Time =
                    !String.IsNullOrEmpty(time)
                        ? time
                        : gr.GetTimeBasedOnSomething(date, (int?)basedOn, timeDifference)
            };
            if (String.IsNullOrEmpty(time))
            {
                e.BasedOn = (int?)basedOn;
                e.TimeDifference = timeDifference;
            }
            if (eventId != null)
            {
                gr.UpdateEvent(eventId.Value,e);
            }
            else
            {
                gr.AddEvent(e);
            }

            return Redirect("/");
        }

        public ActionResult YearView()
        {
            return View();
        }

        public ActionResult SubmitEventType(string eventName, string time, DateTime? startDate, DateTime? endDate, BasedOn basedOn, int timeDifference, IEnumerable<string> restrictions, IEnumerable<string> exclusions,int?eventTypeId)
        {
            var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);

            var et = new EventType
            {
                Name = eventName
            };
            et.StartDate = startDate;
            et.EndDate = endDate;
            if (!String.IsNullOrEmpty(time))
            {
                et.FixedTime = time;
            }
            else
            {
                et.BasedOn = (int?)basedOn;
                et.TimeDifference = timeDifference;
            }
            if (eventTypeId != null)
            {
                gr.UpdateEventType(eventTypeId.Value,et,restrictions,exclusions);
            }
            else
            {
                gr.AddEventType(et, restrictions, exclusions);
            }

            return Redirect("/");
        }


        //public ActionResult Calendar()
        //{
        //    return View();
        //}
    }
}
