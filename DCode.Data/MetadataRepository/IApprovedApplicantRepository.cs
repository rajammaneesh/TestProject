using DCode.Data.DbContexts;
using System.Collections.Generic;

namespace DCode.Data.MetadataRepository
{
    public interface IApprovedApplicantRepository
    {
        IEnumerable<approvedapplicant> GetApprovedApplicants();
    }
}
