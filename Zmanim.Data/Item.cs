using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zmanim.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public bool YomTov { get; set; }//"yyyy-mm-dd"
        public string Title { get; set; }
        public string Memo { get; set; }
        public string Url { get; set; }
    }
}
