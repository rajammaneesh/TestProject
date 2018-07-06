using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
   public class Offering
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Portfolio_Id { get; set; }
        public string Code { get; set; }
        public string RM_Email_Group { get; set; }
        public string Practice_Email_Group { get; set; }
    }
}
