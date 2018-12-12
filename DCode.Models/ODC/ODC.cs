using System;

namespace DCode.Models.ODC
{
    /// <summary>
    /// Individual ODC node containing the details of an ODC
    /// </summary>
    [Serializable]
    public class ODC
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string OfferingId { get; set; }

        public string DistributionList { get; set; }

        public string ExcludeTaskTypeList { get; set; }

        public string Logo { get; set; }

        public string ShowTechX { get; set; }

        public string MailFooterText { get; set; }

        public string MailHeaderImage { get; set; }
    }
}
