using System;

namespace DCode.Models.Base
{
    public interface IViewModel
    {
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string Status { get; set; }
        DateTime? StatusDate { get; set; }
    }
}
