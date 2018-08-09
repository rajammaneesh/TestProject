using DCode.Models.ResponseModels.Common;
using System.Collections.Generic;

namespace DCode.Models.RequestModels
{
    public class ProfileRequest
    {
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmailId { get; set; }
        public bool IsSubscribedToNotifications { get; set; }
        public List<Skill> SkillSet { get; set; }
    }
}
