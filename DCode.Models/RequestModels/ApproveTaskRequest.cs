using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.RequestModels
{
    public class ApproveTaskRequest
    {
        public int TaskApplicantId { get; set; }
        public int TaskId { get; set; }
        public int ApplicantId { get; set; }
    }
}
