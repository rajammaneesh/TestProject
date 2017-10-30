using DCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class RequestorSummary
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get { return FirstName + Constants.Comma + LastName; }
        }
        public string EmailId { get; set; }
        //Do not add more properties, use instead Requestor model
    }
}
