using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zmanim.Data;
using Newtonsoft.Json;

namespace MyShulWorld.Models
{
    public class UpdatesViewModel
    {


        public EventType Et { get; set; }
        public Event E { get; set; }
        // public string TimePickerTime { get; set; }
        private string ViewTime { get; set; }
        private string Time24H { get; set; }
        public List<string> RestrictionsStrings { get; set; }
        public List<string> ExclusionsStrings { get; set; }


        public string EventName()
        {
            if (Et != null)
            {
                return Et.Name;
            }
            if (E != null)
            {
                return E.EventName;
            }
            return null;
        }

        public string RdoRecur()
        {
            if (Et != null)
            {
                return "checked";
            }
            if (E != null)
            {
                return "disabled";
            }
            return null;
        }

        public string RdoOnce()
        {
            if (E != null)
            {
                return "checked";
            }
            if (Et != null)
            {
                return "disabled";
            }
            return null;
        }

        public string RdoFixed()
        {
            if (Et != null && !string.IsNullOrEmpty(Et.FixedTime) && Et.BasedOn == null || E != null && !string.IsNullOrEmpty(E.Time) && E.BasedOn == null)
            {
                return "checked";
            }
            return null;
        }

        public string RdoBasedon()
        {
            if (Et != null && Et.BasedOn != null || E != null && E.BasedOn != null)
            {
                return "checked";
            }
            return null;
        }

        public string RdoBefore()
        {
            if (Et != null && Et.TimeDifference < 0 || E != null && E.TimeDifference < 0)
            {
                return "checked";
            }
            return null;
        }

        public string RdoAfter()
        {
            if (Et != null && Et.TimeDifference > 0 || E != null && E.TimeDifference > 0)
            {
                return "checked";
            }
            return null;
        }

        public string CheckIfBasedon(int selection)
        {
            if (Et != null && Et.BasedOn == selection || E != null && E.BasedOn == selection)
            {
                return "selected";
            }
            return null;
        }

        public string SingleDate()
        {
            if (E != null && E.Date != null)
            {
                return @"value=" + E.Date + @"";
            }
            return null;
        }

        public string StartDate()
        {
            if (Et != null && Et.StartDate != null)
            {
                return @"value=" + Et.StartDate + @"";
            }
            return null;
        }

        public string EndDate()
        {
            if (Et != null && Et.EndDate != null)
            {
                return @"value=" + Et.EndDate + @"";
            }
            return null;
        }

        public string Time24Func()
        {
            if (Et != null && !string.IsNullOrEmpty(Et.FixedTime))
            {
                Time24H = GetTime24H(Et.FixedTime);
                return Time24H;
            }
            if (E != null && !string.IsNullOrEmpty(E.Time))
            {
                Time24H = GetTime24H(E.Time);
                return Time24H;
            }
            return "00:00";
        }

        public string ViewTimeFunc()
        {
            if (Et != null && !string.IsNullOrEmpty(Et.FixedTime))
            {
                ViewTime = GetViewTime(Et.FixedTime);
                return ViewTime;
            }
            if (E != null && !string.IsNullOrEmpty(E.Time))
            {
                ViewTime = GetViewTime(E.Time);
                return ViewTime;
            }
            return "12 : 00 AM";
        }



        public int? Difference()
        {
            if (Et != null && Et.TimeDifference != 0)
            {
                if (Et.TimeDifference < 0)
                {
                    return -Et.TimeDifference;
                }
                return Et.TimeDifference;
            }
            if (E != null && E.TimeDifference != 0)
            {
                if (E.TimeDifference < 0)
                {
                    return -E.TimeDifference;
                }
                return E.TimeDifference;
            }
            return null;
        }

        public string ShowRecur()
        {
            if (Et != null)
            {
                return "recurring-update";
            }
            return null;
        }

        public string ShowDate()
        {
            if (E != null)
            {
                return "single-date-update";
            }
            return null;
        }

        public string ShowTime()
        {
            if (Et != null && Et.FixedTime != null && Et.BasedOn == null || E != null && E.Time != null && E.BasedOn == null)
            {
                return "time-pick-update";
            }
            return null;
        }

        public string ShowBasedOnDifference()
        {
            if (Et != null && Et.BasedOn > 0 || E != null && E.BasedOn > 0)
            {
                return "basedon-difference-update";
            }
            return null;
        }

        public string ShowStartEnd()
        {
            if (Et != null && Et.StartDate != null)
            {
                return "from-to-update";
            }
            return null;
        }

        public string CheckIfSelectedRest(string option, int index)
        {
            if (Et != null && RestrictionsStrings != null)
            {
                if (RestrictionsStrings[index - 1].Contains(option))
                {
                    return "selected";
                }
                return null;
            }
            return null;
        }

        public string CheckIfSelectedExc(string option, int index)
        {
            if (Et != null && ExclusionsStrings != null)
            {
                if (ExclusionsStrings[index - 1].Contains(option))
                {
                    return "selected";
                }
                return null;
            }
            return null;
        }

        private string GetViewTime(string view)
        {
            string orig = view;
            string hour = "";
            string min = "";
            string amPm = "";

            if (orig.Length == 8)
            {
                hour += orig[0];
                hour += orig[1];
                min += orig[3];
                min += orig[4];
                amPm += orig[6];
                amPm += orig[7];
            }
            else
            {
                hour += orig[0];
                min += orig[2];
                min += orig[3];
                amPm += orig[5];
                amPm += orig[6];
            }
            return hour + " : " + min + " " + amPm;
        }

        private string GetTime24H(string time24)
        {
            string orig = time24;
            string hour = "";
            string min = "";
            string amPm = "";

            if (orig.Length == 8)
            {
                hour += orig[0];
                hour += orig[1];
                min += orig[3];
                min += orig[4];
                amPm += orig[6];
                amPm += orig[7];
            }
            else
            {
                hour += orig[0];
                min += orig[2];
                min += orig[3];
                amPm += orig[5];
                amPm += orig[6];
            }

            if (amPm == "PM" && int.Parse(hour) < 12)
            {
                return int.Parse(hour) + 12 + ":" + min;
            } 
            if (amPm == "AM" && int.Parse(hour) == 12)
            {
                return "00:" + min;
            } 
            return hour + ":" + min;
        }

        public string RestId(int i)
        {
            return "rString" + i;
        }

        public string RestDivName(int i)
        {
            return "restriction" + i + "[]";
        }

        public string RestDivId(int i)
        {
            return "restriction" + i;
        }

        public string ExcDivName(int i)
        {
            return "exclusion" + i + "[]";
        }

        public string ExcDivId(int i)
        {
            return "exclusion" + i;
        }
    }
}
