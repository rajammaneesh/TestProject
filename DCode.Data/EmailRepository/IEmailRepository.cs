using DCode.Data.DbContexts;

namespace DCode.Data.EmailRepository
{
    public interface IEmailRepository
    {
        int InsertEmailDetails(email_tracker emailTracker);
    }
}
