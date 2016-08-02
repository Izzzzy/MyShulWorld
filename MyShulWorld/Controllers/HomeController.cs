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
        //
        // GET: /Home/
        public ActionResult EventEntry()
        {
            return View();
        }
    

        public ActionResult SubmitEventsForYear(string year)
        {
            var gr=new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.PopulateEventsForAYear(year,gr.GetAllEventTypes());
            return View();
        }

        public ActionResult SubmitDeleteEvent(int eventId)
        {
            var gr=new GabbaiRepository(Properties.Settings.Default.ConStr);
            gr.DeleteEvent(eventId);
            return View();
        }
    
        public ActionResult Index()
        {
            var gr=new GabbaiRepository(Properties.Settings.Default.ConStr);
            //gr.PopulateShkiaFor30Days();
            return View();
        }

        public ActionResult SubmitEventEntry(bool recurring,string eventName,DateTime date,bool isFixed,string time,BasedOn basedOn,int timeDifference,string identifier,DateTime StartDate,DateTime EndDate)
        {
            var gr=new GabbaiRepository(Properties.Settings.Default.ConStr);
            if (!recurring)
            {
                var e=new Event
                {
                    Date = date,
                    EventName = eventName
                };
                if (isFixed)
                {
                    e.Time = time;
                }
                else
                {
                    e.Time = gr.GetTimeBasedOnSomething(date, (int?)basedOn, timeDifference);
                }
                gr.AddEvent(e);
            }
            else
            {
                var et=new EventType
                {
                    Name=eventName,
                    Identifier=identifier,
                    StartDate=StartDate,
                    EndDate=EndDate
                };
                if (isFixed)
                {
                    et.FixedTime = time;
                }
                else
                {
                    et.BasedOn = (int?) basedOn;
                    et.TimeDifference = timeDifference;
                }
                gr.AddEventType(et);
            }
            return Redirect("/");
        }

        //public ActionResult SubmitEvent(string eventName, DateTime date, bool isFixed, string time, BasedOn basedOn, int timeDifference, string identifier)
        //{
        //    var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            
        //        gr.AddEvent(new Event
        //        {
        //            Date = date,
        //            EventName = eventName,
        //            Time = time
        //        });
            
        //        var e = new Event
        //        {
        //            EventName = eventName,
        //        };

        //        if (isFixed)
        //        {
        //            e.Time = time;
        //        }
        //        else
        //        {
        //            e.Time = basedOn.ToString();
                    
        //        }
        //        gr.AddEvent(e);
            
        //    return Redirect("/");
        //}

        //public ActionResult SubmitEventType(string eventName, bool isFixed, string time, BasedOn basedOn, int timeDifference, string identifier)
        //{
        //    var gr = new GabbaiRepository(Properties.Settings.Default.ConStr);
            
        //        var et = new EventType
        //        {
        //            Name = eventName,
        //            Identifier = identifier
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
            
        //    return Redirect("/");
        //}


        //public ActionResult Calendar()
        //{
        //    return View();
        //}
    }
}
