using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace Zmanim.Data
{
    public class GabbaiRepository
    {
        private string _connectionString;

        public GabbaiRepository(string cn)
        {
            _connectionString = cn;
        }

        public void DeleteAllEventsAndEventTypes()
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Exclusions.DeleteAllOnSubmit(context.Exclusions);
                context.Restrictions.DeleteAllOnSubmit(context.Restrictions);
                context.SubmitChanges();
                context.EventTypes.DeleteAllOnSubmit(context.EventTypes);
                context.Events.DeleteAllOnSubmit(context.Events);
                context.SubmitChanges();
            }
        }

        private void DeleteRestrictonsForEventType(int eventTypeId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Restrictions.DeleteAllOnSubmit(context.Restrictions.Where(r => r.EventTypeId == eventTypeId));
                context.SubmitChanges();
            }
        }

        private void DeleteExclusionsForEventType(int eventTypeId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Exclusions.DeleteAllOnSubmit(context.Exclusions.Where(e => e.EventTypeId == eventTypeId));
                context.SubmitChanges();
            }
        }

        private void AddRestrictionsAndExclusions(int eventTypeId, IEnumerable<string> restrictions,
            IEnumerable<string> exclusions)
        {
            var res = new List<Restriction>();
            var exc = new List<Exclusion>();
            if (restrictions != null)
            {
                //res = (List<Restriction>) restrictions.Select(r => new Restriction { Restriction1 = r, EventTypeId = eventType.ID });
                foreach (string s in restrictions)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        res.Add(new Restriction
                        {
                            Restriction1 = s,
                            EventTypeId = eventTypeId
                        });
                    }
                }
            }
            if (exclusions != null)
            {
                //exc = (List<Exclusion>) exclusions.Select(e => new Exclusion { Exclusion1 = e, EventTypeId = eventType.ID });

                foreach (string s in exclusions)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        exc.Add(new Exclusion
                        {
                            Exclusion1 = s,
                            EventTypeId = eventTypeId
                        });
                    }

                }
            }

            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Restrictions.InsertAllOnSubmit(res);
                context.Exclusions.InsertAllOnSubmit(exc);
                context.SubmitChanges();
            }
        }

        public void UpdateEventType(int eventTypeId, EventType et, IEnumerable<string> restrictions,
            IEnumerable<string> exclusions)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                //var events = context.Events.Where(e => e.EventTypeId == eventTypeId && e.Date > DateTime.Now);
                //context.Events.DeleteAllOnSubmit(events);
                DateTime dt = et.LastDayPopulated.Value;
                DeleteEvents(DateTime.Now, et.LastDayPopulated.Value, et.ID);
                DeleteRestrictonsForEventType(et.ID);
                DeleteExclusionsForEventType(et.ID);
                EventType eventType = context.EventTypes.FirstOrDefault(e => e.ID == eventTypeId);
                if (eventType != null)
                {
                    eventType.Name = et.Name;
                    eventType.Fixed = et.Fixed;
                    eventType.FixedTime = et.FixedTime;
                    eventType.BasedOn = et.BasedOn;
                    eventType.TimeDifference = et.TimeDifference;
                    eventType.StartDate = et.StartDate < DateTime.Now ? DateTime.Now : et.StartDate;
                    eventType.EndDate = et.EndDate;
                    //eventType.LastDayPopulated = et.LastDayPopulated;//maybe set LastDayPopulated to today and save previous
                    //LastDay in a variable to know where to populate til
                }
                context.SubmitChanges();

                AddRestrictionsAndExclusions(et.ID,restrictions,exclusions);
                MakeSureEventsAreUpToDate(dt, et);
                
            }
        }

        private void MakeSureEventsAreUpToDate(DateTime date, EventType et)
        {
            if (et.LastDayPopulated > date)
            {
                DeleteEvents(et.LastDayPopulated.Value, date, et.ID);
            }
            if (et.LastDayPopulated < date)
            {
                GenerateInsertionOfEvents(et.LastDayPopulated.Value.AddDays(1), date, et);
            }
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                EventType eventType = context.EventTypes.FirstOrDefault(t => t.ID == et.ID);
                eventType.LastDayPopulated = date;
                context.SubmitChanges();
            }
        }

        public void AddEventType(EventType eventType, IEnumerable<string> restrictions, IEnumerable<string> exclusions)
        {
            if (eventType.StartDate == null)
            {
                eventType.StartDate = DateTime.Now;
            }
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.EventTypes.InsertOnSubmit(eventType);
                context.SubmitChanges();
            }
            AddRestrictionsAndExclusions(eventType.ID, restrictions, exclusions);
            //var res = new List<Restriction>();
            //var exc = new List<Exclusion>();
            //if (restrictions != null)
            //{
            //    //res = (List<Restriction>) restrictions.Select(r => new Restriction { Restriction1 = r, EventTypeId = eventType.ID });
            //    foreach (string s in restrictions)
            //    {
            //        if (!String.IsNullOrEmpty(s))
            //        {
            //            res.Add(new Restriction
            //        {
            //            Restriction1 = s,
            //            EventTypeId = eventType.ID
            //        });
            //        }
            //    }

            //}
            //if (exclusions != null)
            //{
            //    //exc = (List<Exclusion>) exclusions.Select(e => new Exclusion { Exclusion1 = e, EventTypeId = eventType.ID });

            //    foreach (string s in exclusions)
            //    {
            //        if (!String.IsNullOrEmpty(s))
            //        {
            //            exc.Add(new Exclusion
            //        {
            //            Exclusion1 = s,
            //            EventTypeId = eventType.ID
            //        });
            //        }

            //    }
            //}

            //using (var context = new MyShulWorldDBDataContext(_connectionString))
            //{
            //    context.Restrictions.InsertAllOnSubmit(res);
            //    context.Exclusions.InsertAllOnSubmit(exc);
            //    context.SubmitChanges();
            //}
            MakeSureEventsAreUpToDate(DateTime.Now, eventType);
        }

        public void DeleteEventType(int eventTypeId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Restrictions.DeleteAllOnSubmit(context.Restrictions.Where(r => r.EventTypeId == eventTypeId));
                context.Exclusions.DeleteAllOnSubmit(context.Exclusions.Where(e => e.EventTypeId == eventTypeId));
                context.SubmitChanges();

                context.EventTypes.DeleteOnSubmit(GetEventTypeById(eventTypeId));
                var events = context.Events.Where(e => e.EventTypeId == eventTypeId && e.Date > DateTime.Now);
                context.Events.DeleteAllOnSubmit(events);
                context.SubmitChanges();
            }
        }


        public void AddEvent(Event e)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Events.InsertOnSubmit(e);
                context.SubmitChanges();
            }
        }

        public void DeleteEvent(int eventId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Events.DeleteOnSubmit(GetEventById(eventId));
                context.SubmitChanges();
            }
        }

        private void DeleteEvents(DateTime startDate, DateTime endDate, int eventTypeId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                context.Events.DeleteAllOnSubmit(context.Events.Where(e => e.EventTypeId == eventTypeId && e.Date >= startDate && e.Date <= endDate));
                context.SubmitChanges();

                var et = context.EventTypes.FirstOrDefault(t => t.ID == eventTypeId);
                if (endDate >= et.LastDayPopulated.Value)
                {
                    et.LastDayPopulated = startDate.AddDays(-1);
                    context.SubmitChanges();
                }
            }
        }

        public void UpdateEvent(int eventId, Event e)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                Event a = context.Events.FirstOrDefault(i => i.Id == eventId);
                a.EventName = e.EventName;
                a.EventTypeId = e.EventTypeId;
                a.Date = e.Date;
                a.Time = e.Time;
                context.SubmitChanges();
            }
        }


        private EventType GetEventTypeById(int eventTypeId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                return context.EventTypes.FirstOrDefault(et => et.ID == eventTypeId);
            }
        }


        public string GetTimeBasedOnSomething(DateTime date, int? basedOn, double minutesDifference)
        {
            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            DateTime dateTime = new DateTime();
            if (basedOn == (int)BasedOn.Sunset)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetSunset() != null)
                {
                    dateTime = (DateTime)zc.GetSunset();
                }
            }
            else if (basedOn == (int)BasedOn.Chatzos)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetChatzos() != null)
                {
                    dateTime = (DateTime)zc.GetChatzos();
                }
            }
            else if (basedOn == (int)BasedOn.Alos)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetAlosHashachar() != null)
                {
                    dateTime = (DateTime)zc.GetAlosHashachar();
                }
            }
            else if (basedOn == (int)BasedOn.SofZmanShmaGRA)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetSofZmanShmaGRA() != null)
                {
                    dateTime = (DateTime)zc.GetSofZmanShmaGRA();
                }
            }
            else if (basedOn == (int)BasedOn.SofZmanShmaGRA)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetSofZmanShmaGRA() != null)
                {
                    dateTime = (DateTime)zc.GetSofZmanShmaGRA();
                }
            }
            else if (basedOn == (int)BasedOn.SofZmanShmaMGA)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetSofZmanShmaMGA() != null)
                {
                    dateTime = (DateTime)zc.GetSofZmanShmaMGA();
                }
            }
            else if (basedOn == (int)BasedOn.PlagHamincha)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetPlagHamincha() != null)
                {
                    dateTime = (DateTime)zc.GetPlagHamincha();
                }
            }
            else if (basedOn == (int)BasedOn.MinchaGedola)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetMinchaGedola() != null)
                {
                    dateTime = (DateTime)zc.GetMinchaGedola();
                }
            }
            else if (basedOn == (int)BasedOn.MinchaKetana)
            {
                ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);

                if (zc.GetMinchaKetana() != null)
                {
                    dateTime = (DateTime)zc.GetMinchaKetana();
                }
            }
            return dateTime.AddMinutes(minutesDifference).ToShortTimeString();
        }

        public void PopulateEventsUntilDate(DateTime date)
        {
            foreach (EventType et in GetAllEventTypes())
            {
                MakeSureEventsAreUpToDate(date, et);
            }
        }


        public void PopulateEventsForAYear(string year, IEnumerable<EventType> eventTypes)
        {
            foreach (EventType et in eventTypes)
            {
                if (et.LastDayPopulated >= DateTime.Parse(year + "-12-31"))
                {
                    continue;
                }
                DateTime d = DateTime.Parse(year + "-01-01");
                if (et.LastDayPopulated > DateTime.Parse(year + "-01-01"))
                {
                    d = (DateTime)et.LastDayPopulated;
                }
                if (d < DateTime.Now)
                {
                    d = DateTime.Now;
                }
                //GenerateInsertionOfEventsForYear(year, et);
                GenerateInsertionOfEvents(d, DateTime.Parse(year + "-12/31"), et);
                using (var context = new MyShulWorldDBDataContext(_connectionString))
                {
                    var firstOrDefault = context.EventTypes.FirstOrDefault(e => e.ID == et.ID);
                    if (firstOrDefault != null)
                        firstOrDefault.LastDayPopulated = DateTime.Parse(year + "-12/31");

                    context.SubmitChanges();
                }
            }
        }

        private void GenerateInsertionOfEvents(DateTime start, DateTime end, EventType eventType)
        {
            HebcalItems items = GetItemsBetweenDates(start, end);
            //HebcalItems items = new HebcalItems
            //{
            //    Items = (Item[])GetItemsBetweenDates(start, end)
            //};

            for (DateTime x = start; x <= end.AddDays(1); x = x.AddDays(1))
            {
                //var x1 = x;
                if (IsApplicable(x, GetTypesOfDay(x, items.Items.Where(i => (DateTime.Parse(i.Date)).Date == x.Date)), eventType))
                {
                    Event e = new Event
                    {
                        Date = x,
                        EventName = eventType.Name,
                        EventTypeId = eventType.ID,
                        BasedOn = eventType.BasedOn,
                        TimeDifference = eventType.TimeDifference
                    };
                    //may be more performent to wrap this whole "for" loop (beginning line 97) in the following "if" statement and to repeat all the
                    //logic again in the following "else if" cuz then only has to check once
                    if (eventType.FixedTime != null)
                    {
                        e.Time = eventType.FixedTime;
                    }
                    else if (eventType.BasedOn != null)
                    {
                        double minutes = eventType.TimeDifference != null ? eventType.TimeDifference.Value : 0;
                        e.Time = GetTimeBasedOnSomething(x, eventType.BasedOn, minutes);
                    }
                    AddEvent(e);
                }
            }
        }


        private IEnumerable<string> GetTypesOfDay(DateTime date, IEnumerable<Item> itemsForDate)
        {
            List<string> types = new List<string>();
            types.Add(date.DayOfWeek.ToString());
            if (itemsForDate.Any(i => i.YomTov))
            {
                types.Add("YomTov");
            }
            if (itemsForDate.Any(i => i.Category == "roshchodesh"))
            {
                types.Add("RoshChodesh");
            }
            if (itemsForDate.Any(i => i.Title.Contains("CH''M")))
            {
                types.Add("CholHamoed");
            }
            if (itemsForDate.Any(i => i.Title == "Yom Kippur"))
            {
                types.Add("YomKippur");
            }
            if (itemsForDate.Any(i => i.Title == "Rosh Hashana"))
            {
                types.Add("RoshHashana");
            }
            if (itemsForDate.Any(i => i.Title == "Tish'a B'av"))
            {
                types.Add("TishaB'av");
            }
            //might not be so efficient
            if (itemsForDate.Any(i => i.Memo.Contains("fast") && i.Title != "Yom Kippur" && i.Memo != "Fast of the First Born"))
            {
                types.Add("Taanis");
            }
            if (itemsForDate.Any(i => i.Title.Contains("Chanukah")))
            {
                types.Add("Chanuka");
            }
            return types;
        }


        private bool IsApplicable(DateTime date, IEnumerable<string> typesOfDay, EventType et)
        {
            if (et.EndDate != null && ((date < et.StartDate) || (date > et.EndDate.Value.AddDays(1))))
            {
                return false;
            }

            List<Restriction> restrictions = new List<Restriction>();
            List<Exclusion> exclusions = new List<Exclusion>();
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                foreach (Restriction r in context.Restrictions.Where(r => r.EventTypeId == et.ID))
                {
                    restrictions.Add(r);
                }
                foreach (Exclusion e in context.Exclusions.Where(e => e.EventTypeId == et.ID))
                {
                    exclusions.Add(e);
                }
            }
            if (restrictions.Any())
            {
                foreach (var r in restrictions)
                {
                    if (!typesOfDay.Any(tod => r.Restriction1.Contains(tod)))
                    {
                        return false;
                    }
                }
            }
            if (exclusions.Any())
            {
                foreach (var e in exclusions)
                {
                    if (!typesOfDay.Any(tod => e.Exclusion1.Contains(tod)))
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }


        //private IEnumerable<Item> GetItemsBetweenDates(DateTime start, DateTime end)
        private HebcalItems GetItemsBetweenDates(DateTime start, DateTime end)
        {
            List<Item> items = new List<Item>();
            for (int x = start.Year; x <= end.Year; x++)
            {
                using (var webclient = new WebClient())
                {
                    string json =
                        webclient.DownloadString("https://www.hebcal.com/hebcal/?cfg=json&v=1&year=" + x +
                                                 "&i=off&maj=on&min=on&nx=on&mf=on&ss=on&lg=s");
                    ;
                    //List<Item> itemss = JsonConvert.DeserializeObject<List<Item>>(json);
                    HebcalItems hEBCALitems = JsonConvert.DeserializeObject<HebcalItems>(json);
                    foreach (Item i in hEBCALitems.Items)
                    {
                        if (DateTime.Parse(i.Date) >= start && DateTime.Parse(i.Date) <= end)
                        {
                            items.Add(i);
                        }
                    }
                }
            }
            HebcalItems it = new HebcalItems();
            it.Items = items.ToArray();
            //return items;
            return it;
        }

        private IEnumerable<Item> GetItemsForYear(string year)
        {
            using (var webclient = new WebClient())
            {
                string json =
                    webclient.DownloadString("https://www.hebcal.com/hebcal/?cfg=json&v=1&year=" + year +
                                             "&i=off&maj=on&min=on&nx=on&mf=on&ss=on&lg=s");
                return JsonConvert.DeserializeObject<Item[]>(json);
            }
        }


        public IEnumerable<CalendarEvent> GetEventsBetweenDates(string startDate, string endDate)
        {
            List<CalendarEvent> cEvents = new List<CalendarEvent>();
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                foreach (Event e in context.Events.Where(e => e.Date > DateTime.Parse(startDate) && e.Date < DateTime.Parse(endDate)))
                {
                    //string time = e.Time;
                    //if (e.Time[1] != ':')
                    //{
                    //    time = "0" + e.Time;
                    //}
                    //if (time[6] == 'P')
                    //{
                    //    string s = "";
                    //    s += (int.Parse(time[0].ToString()) + 1).ToString();
                    //    s += (int.Parse(time[1].ToString()) + 2).ToString();
                    //    //time.Remove(0) = s[0];
                    //    time = time.Substring(time.IndexOf(':'));
                    //    time = (int.Parse(time[1].ToString()) + 2).ToString() + time;
                    //    time = (int.Parse(time[0].ToString()) + 1).ToString() + time;
                    //}
                    DateTime d = (DateTime)e.Date;
                    cEvents.Add(new CalendarEvent
                    {
                        Title = e.EventName + " " + e.Time,
                        Start = d.ToString("yyyy-MM-dd")// + "T" + time//"14:30:00" //e.Time.Replace(" AM",":00")

                    });
                }
                return cEvents;
            }
        }

        public Event GetEventById(int eventId)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                return context.Events.FirstOrDefault(e => e.Id == eventId);
            }
        }

        public IEnumerable<EventType> GetAllEventTypes()
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                return context.EventTypes.ToList();
            }
        }

        public IEnumerable<Event> GetEventsForDay(DateTime date)
        {
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                return context.Events.Where(e => e.Date == date).ToList();
            }
        }









        //private bool IsApplicable(DateTime date, Item itemForDate, EventType et)
        //{
        //    if ((et.StartDate != null && date < et.StartDate) || (et.EndDate != null && date > et.EndDate))
        //    {
        //        return false;
        //    }

        //    if (itemForDate != null)
        //    {
        //        if (et.TypesOfDaysApplicable.Contains(GetTypeOfDay(itemForDate)))
        //        {
        //            return et.DaysOfWeekApplicable.Contains(date.DayOfWeek.ToString());
        //        }
        //    }
        //    return et.Identifier.Contains(date.DayOfWeek.ToString());
        //}


        //private void GenerateInsertionOfEventsForYear(string year, EventType eventType)
        //{
        //    HebcalItems items = new HebcalItems
        //    {
        //        Items = (Item[])GetItemsForYear(year)
        //    };

        //    for (DateTime x = DateTime.Parse(year + "/01/01"); x < DateTime.Parse(year + "/12/31"); x.AddDays(1))
        //    {
        //        if (IsApplicable(x, items.Items.FirstOrDefault(i => DateTime.Parse(i.Date) == x), eventType))
        //        {
        //            Event e = new Event
        //            {
        //                Date = x,
        //                EventName = eventType.Name,
        //                EventTypeId = eventType.ID,
        //            };
        //            //may be more performent to wrap this whole "for" loop (beginning line 97) in the following "if" statement and to repeat all the
        //            //logic again in the following "else if" cuz then only has to check once
        //            if (eventType.FixedTime != null)
        //            {
        //                e.Time = eventType.FixedTime;
        //            }
        //            else if (eventType.BasedOn != null)
        //            {
        //                double minutes = eventType.TimeDifference.Value;
        //                e.Time = GetTimeBasedOnSomething(x, eventType.BasedOn, minutes);
        //            }
        //            AddEvent(e);
        //        }
        //    }
        //    using (var context = new MyShulWorldDBDataContext(_connectionString))
        //    {
        //        //add the year to eventType.YearsPopulated
        //        context.EventTypes.FirstOrDefault(e => e.ID == eventType.ID).YearsPopulated += year + ",";
        //        context.SubmitChanges();
        //    }
        //}

        //private bool IsApplicable(DateTime date, Item itemForDate, EventType et)
        //{
        //    if ((et.StartDate != null && date < et.StartDate) || (et.EndDate != null && date > et.EndDate))
        //    {
        //        return false;
        //    }
        //    //foreach (Item i in itemsForDate)
        //    if (itemForDate != null)
        //    {

        //        //or do .contains
        //        if (et.Identifier == "YomTov" && (!itemForDate.Title.Contains("Rosh Hashana") && !itemForDate.Title.Contains("Yom Kippur")))
        //        {
        //            return itemForDate.YomTov;
        //        }
        //        if (et.Identifier.Contains("YomKippur"))
        //        {
        //            return itemForDate.Title.Contains("Yom Kippur");
        //        }
        //        if (et.Identifier.Contains("RoshHashana"))
        //        {
        //            return itemForDate.Title.Contains("Rosh Hashana");
        //        }
        //        if (et.Identifier.Contains("ExclusivelyShabbos"))
        //        {
        //            return DateTime.Parse(itemForDate.Date).DayOfWeek == DayOfWeek.Saturday;
        //        }

        //        //special days that are regardless of day of week
        //        if (et.Identifier.Contains("TishaB'av"))
        //        {
        //            return itemForDate.Title == "Tish'a B'av";
        //        }
        //        //add more special days...

        //        if (et.Identifier.Contains("ExclusivelyRoshChodesh"))
        //        {
        //            return itemForDate.Category.Contains("roshchodesh") &&
        //                   et.Identifier.Contains(DateTime.Parse(itemForDate.Date).DayOfWeek.ToString());
        //        }
        //        if (!et.Identifier.Contains("RoshChodesh"))
        //        {
        //            return !itemForDate.Category.Contains("roshchodesh") &&
        //                   et.Identifier.Contains(DateTime.Parse(itemForDate.Date).DayOfWeek.ToString());
        //        }
        //        if (et.Identifier.Contains("ExclusivelyTaanis"))
        //        {
        //            return itemForDate.Memo.Contains("fast") && !itemForDate.Title.Contains("Yom Kippur") && !itemForDate.Title.Contains("Tish'a B'av") &&
        //                   et.Identifier.Contains(DateTime.Parse(itemForDate.Date).DayOfWeek.ToString());
        //        }
        //        if (!et.Identifier.Contains("Taanis"))
        //        {
        //            return !itemForDate.Memo.Contains("fast") && !itemForDate.Title.Contains("Yom Kippur") && !itemForDate.Title.Contains("Tish'a B'av") &&
        //                   et.Identifier.Contains(DateTime.Parse(itemForDate.Date).DayOfWeek.ToString());
        //        }
        //    }
        //    return et.Identifier.Contains(date.DayOfWeek.ToString());
        //    //return identifier.Contains(DateTime.Parse(itemForDate.Date).DayOfWeek.ToString());
        //}

        //Before found better json link:
        //private IEnumerable<Day> GetTypesOfDays(string year)
        //{
        //    using (var webclient = new WebClient())
        //    {
        //        string json = webclient.DownloadString("https://www.hebcal.com/hebcal/?cfg=fc&v=1&start="+year+"-01-01&end="+year+"-12-31&i=off&zip=08701&maj=on&min=on&nx=on&mf=on&ss=on&mod=on&lg=s&s=on");
        //        return JsonConvert.DeserializeObject<Day[]>(json);
        //    }
        //}

        //public void AddEventBasedOnSomething(DateTime date, EventType eventType)
        //{
        //    string locationName = "Lakewood, NJ";
        //    double latitude = 40.09596; //Lakewood, NJ
        //    double longitude = -74.22213; //Lakewood, NJ
        //    double elevation = 0; //optional elevation
        //    ITimeZone timeZone = new OlsonTimeZone("America/New_York");
        //    GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
        //    if (eventType.BasedOn != null && eventType.BasedOn == (int)BasedOn.Sunset)
        //    {
        //        ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);
        //        DateTime dateTime = new DateTime();
        //        double diff = eventType.TimeDifference.Value;
        //        if (zc.GetSunset() != null)
        //        {
        //            dateTime = (DateTime)zc.GetSunset();
        //        }
        //        AddEvent(new Event
        //        {
        //            Date = date,
        //            EventName = eventType.Name,
        //            EventTypeId = eventType.ID,
        //            Time = dateTime.AddMinutes(diff).ToShortTimeString()
        //        });
        //    }
        //}

        private void GenerateInsertionOfEventsForYear(string year, EventType eventType)
        {
            HebcalItems items = new HebcalItems
            {
                Items = (Item[])GetItemsForYear(year)
            };

            for (DateTime x = DateTime.Parse(year + "/01/01"); x < DateTime.Parse(year + "/12/31"); x.AddDays(1))
            {
                if (IsApplicable(x, GetTypesOfDay(x, items.Items.Where(i => DateTime.Parse(i.Date) == x)), eventType))
                {
                    Event e = new Event
                    {
                        Date = x,
                        EventName = eventType.Name,
                        EventTypeId = eventType.ID,
                    };
                    //may be more performent to wrap this whole "for" loop (beginning line 97) in the following "if" statement and to repeat all the
                    //logic again in the following "else if" cuz then only has to check once
                    if (eventType.FixedTime != null)
                    {
                        e.Time = eventType.FixedTime;
                    }
                    else if (eventType.BasedOn != null)
                    {
                        double minutes = eventType.TimeDifference != null ? eventType.TimeDifference.Value : 0;
                        e.Time = GetTimeBasedOnSomething(x, eventType.BasedOn, minutes);
                    }
                    AddEvent(e);
                }
            }
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                //add the year to eventType.YearsPopulated
                context.EventTypes.FirstOrDefault(e => e.ID == eventType.ID).LastDayPopulated = DateTime.Parse(year + "/12/31");
                context.SubmitChanges();
            }
        }

        private IEnumerable<Item> GetItemsForDate(DateTime d, HebcalItems items)
        {
            return items.Items.Where(i => DateTime.Parse(i.Date) == d);
        }

        private string GetTypeOfDay(Item ifd)
        {
            if (ifd.YomTov && !ifd.Title.Contains("Rosh Hashana") && !ifd.Title.Contains("Yom Kippur"))
            {
                return "YomTov";
            }
            if (ifd.Category == "roshchodesh")
            {
                return "RoshChodesh";
            }
            if (ifd.Title.Contains("CH''M"))
            {
                return "CholHamoed";
            }
            if (ifd.Title == "Yom Kippur")
            {
                return "YomKippur";
            }
            if (ifd.Title == "Rosh Hashana")
            {
                return "RoshHashana";
            }
            if (ifd.Title == "Tish'a B'av")
            {
                return "TishaB'av";
            }
            //might not be so efficient
            if (ifd.Memo.Contains("fast") && ifd.Title != "Yom Kippur" && ifd.Title != "Tish'a B'av" && ifd.Memo != "Fast of the First Born")
            {
                return "Taanis";
            }
            return "RegularDay";
        }

        public void PopulateShkiaFor30Days()
        {
            var date = DateTime.Now;

            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            //ComplexZmanimCalendar zc = new ComplexZmanimCalendar(location);
            //optionally set it to a specific date with a year, month and day
            //ComplexZmanimCalendar zc = new ComplexZmanimCalendar(new DateTime(1969, 2, 8), location);
            using (var context = new MyShulWorldDBDataContext(_connectionString))
            {
                for (int x = 0; x < 30; x++)
                {
                    ComplexZmanimCalendar zc = new ComplexZmanimCalendar(date, location);
                    DateTime dateTime = (DateTime)zc.GetSunset();
                    //TimeSpan time = (TimeSpan)dateTime.ToString("h:mm tt");
                    string time = dateTime.ToShortTimeString();
                    //context.Events.InsertOnSubmit(new Event
                    //{
                    //    EventName="Shkia",
                    //    Time = time,
                    //    Date=date
                    //});
                    AddEvent(new Event
                    {
                        EventName = "Shkia",
                        Time = time,
                        Date = date
                    });
                    date = date.AddDays(1);
                    //context.SubmitChanges();
                }

            }
        }

    }
}
