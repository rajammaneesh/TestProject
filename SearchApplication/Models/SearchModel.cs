using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchApplication.Models
{
    public class SearchModel
    {
        public int? Task_Type_id { get; set; }
        public int? Offerings_id { get; set; }
        //public int? Service_Line_id { get; set; }
        public string Status { get; set; }
    }
}