using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class PermissionsTask
    {
        public Models.ResponseModels.Task.Task Task { get; set; }
        public Models.ResponseModels.Contributor.Contributor Applicant { get; set; }
        public int TaskApplicantId { get; set; }
        //public PermissionsSummary Applicant { get; set; }
    }
}
