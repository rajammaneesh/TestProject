using DCode.Models.ODC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.Common
{
    public interface IODCService
    {
        ExistingODCList GetExistingODCList(string xmlFilePath);

        ODC GetExistingODCByOfferingId(string xmlFilePath, string offeringId);

        void SetODCAccess(Models.User.UserContext _userContext);
    }
}
