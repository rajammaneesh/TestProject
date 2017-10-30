using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Common
{
    public static class EnumHelper
    {
        public static Enums.ApplicantStatus ConvertToEnumApplicantStatus(string applicantStatus)
        {
            Enums.ApplicantStatus status;
            switch(applicantStatus.ToLowerInvariant())
            {
                case "active":
                    status = Enums.ApplicantStatus.Active;
                    break;
                default:
                    status = Enums.ApplicantStatus.Active;
                    break;
            }
            return status;
        }
    }
}
