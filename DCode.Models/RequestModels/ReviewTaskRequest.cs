using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.RequestModels
{
    public class ReviewTaskRequest
    {
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
        public int ApprovedApplicantId { get; set; }
        public string Rating { get; set; }
        public string Comments { get; set; }
        public bool WorkAgain { get; set; }
    }
}
