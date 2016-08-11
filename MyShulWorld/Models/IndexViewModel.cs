using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zmanim.Data;

namespace MyShulWorld.Models
{
    public class IndexViewModel
    {
        public IEnumerable<EventType> AllEventTypes { get; set; } 
    }
}