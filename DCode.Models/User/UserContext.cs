using DCode.Models.ResponseModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DCode.Models.Enums.Enums;

namespace DCode.Models.User
{
    public class UserContext
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        public bool IsCoreRoleRequestor { get; set; }
        public string Name
        {
            get
            {
                return (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName)) ? string.Empty : $"{FirstName} {LastName}";
            }
        }
        public string Designation { get; set; }
        public string EmailId { get; set; }
        public string EmployeeId { get; set; }
        public List<Skill> SkillSet { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public string ShortName
        {
            get
            {
                if (Name.Trim().Contains(" "))
                {
                    var split = Name.Split(' ');
                    return split[0].Substring(0, 1) + string.Empty + split[1].Substring(0, 1);
                }
                else
                {
                    return Name.Substring(0, 2);
                }
            }
        }
        public string TelephoneNumber { get; set; }
        public string Department { get; set; }
        public string DepartmentCode
        {
            get
            {
                if (!Department.Contains("USI"))
                {
                    return null;
                }

                var departmentCode = Department.Substring(0, Department.IndexOf("USI"))?.Trim();

                return departmentCode;
            }
        }
        public string MobileNumber { get; set; }
        //TBD - Only for Testing
        public string ManagerEmailId { get; set; }
        public string ManagerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }

        public string MsArchiveName { get; set; }

        public bool IsSubscribedToNotifications { get; set; }
    }
}
