using DCode.Models.ResponseModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.RequestModels
{
    public class ProfileRequest
    {
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmailId { get; set; }
        public List<Skill> SkillSet { get; set; }
    }
}
