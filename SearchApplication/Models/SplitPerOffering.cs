using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SearchApplication.Models
{
    public class SplitPerOffering
    {
        public string Code { get; set; }

        public int TasksApplied { get; set; }
    }
}