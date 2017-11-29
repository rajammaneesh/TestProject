using System.Collections.Generic;

namespace DCode.Data.UserRepository
{
    //TODO:(RItwik):: Duplicate IUserRepository
    public interface IUserRepository
    {
        IEnumerable<string> GetSubscribedUserForTask(string task);
    }
}
