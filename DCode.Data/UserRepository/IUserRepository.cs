using System.Collections.Generic;

namespace DCode.Data.UserRepository
{
    public interface IUserRepository
    {
        IEnumerable<string> GetSubscribedUserForTask(string task);
    }
}
