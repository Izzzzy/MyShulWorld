using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zmanim.Data;

namespace MyShulWorld.Models
{
    public class UpdatesViewModel
    {
        public EventType Et { get; set; }
        public Event E { get; set; }
        public bool EventUpdate { get; set; }
        public bool EvetnTypeUpdate { get; set; }

        //for testing
        public List<string> TestList { get; set; }
        public List<string> Restrictions { get; set; }
        public List<string> Exclusions { get; set; }

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

        public string Time()
        {
            if (Et != null && !string.IsNullOrEmpty(Et.FixedTime))
            {

                return Et.FixedTime;
            }
            if (E != null && !string.IsNullOrEmpty(E.Time))
            {
                return E.Time;
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

        public string CheckIfSelected(string option, int index)
        {
            if (Et != null)
            {
                if (Restrictions[index - 1].Contains(option))
                {
                    return "selected";
                }
                return null;
            }
            return null;
        }
    }
}