﻿using DCode.Common;
using DCode.Models.ResponseModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.User
{
    public class UserContext
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Enums.Role Role { get; set; }
        public bool IsCoreRoleRequestor { get; set; }
        public string Name 
        {
            get 
            { 
                return (FirstName.Equals(string.Empty) && LastName.Equals(string.Empty)) ? string.Empty : FirstName + Constants.Space + LastName; 
            } 
        }
        public string Designation { get; set; }
        public string EmailId { get; set; }
        public string EmployeeId { get; set; }
        public List<Skill> SkillSet { get;set;}
        public List<MenuItem> MenuItems { get; set; }
        public string ShortName
        {
            get
            {
                if (Name.Trim().Contains(Constants.Space))
                {
                    var split = Name.Split(Constants.SpaceChar);
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
        public string MobileNumber { get; set; }
        //TBD - Only for Testing
        public string ManagerEmailId { get; set; }
        public string ManagerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
    }
}
