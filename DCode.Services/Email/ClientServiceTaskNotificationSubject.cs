using DCode.Models.Email;

namespace DCode.Services.Email
{
    public class ClientServiceTaskNotificationSubject : ITaskNotificationSubject
    {
        public string Skill { get; set; }
    }
}
