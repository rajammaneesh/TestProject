using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Data.EmailRepository
{
    public interface IEmailRepository
    {
        int InsertEmailDetails(emailtracker emailTracker);
    }
}
