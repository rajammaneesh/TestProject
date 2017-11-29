using static DCode.Models.Enums.Enums;

namespace DCode.Common
{
    public static class EnumHelper
    {
        public static ApplicantStatus ConvertToEnumApplicantStatus(string applicantStatus)
        {
            ApplicantStatus status;
            switch(applicantStatus.ToLowerInvariant())
            {
                case "active":
                    status = ApplicantStatus.Active;
                    break;
                default:
                    status = ApplicantStatus.Active;
                    break;
            }
            return status;
        }
    }
}
