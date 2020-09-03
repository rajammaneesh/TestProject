﻿using System.ComponentModel;

namespace DCode.Models.Enums
{

    public static class Enums
    {
        public enum Role { Admin, Requestor, Contributor };
        public enum TaskStatus { Active, Assigned, Closed };
        public enum TaskType { ClientService = 1, FirmInitiative = 2, ClientServiceCollective = 3 };
        public enum ApplicantStatus { Active, ManagerApproved, Assigned, ManagerRejected, Closed };
        public enum ActionType { Insert, Update };
        public enum SortOrder { ASC, DESC };
        public enum TaskStatusSortFields { Name, Hours, TaskName, ProjectName, DueDate };
        public enum Rating { Good = 1, Average, Unsatisfactory };
        public enum SkillStatus { Active, Closed };
        public enum UserStatus { Active, Closed };
        public enum TaskApplicant { Active, ManagerApproved, Assigned, Closed };

        public enum ApprovedApplicantStatus { Active, Closed };
        public enum PermissionsSortFields { Name, Hours, TaskName, ProjectName, OnBoardingDate };
        public enum EmailType { Approved, Rejected }
        public enum LocationEnum
        {
            [Description("Hyderabad")]
            Hyderabad,
            [Description("Gurgaon")]
            Gurgaon,
            [Description("Mumbai")]
            Mumbai,
            [Description("Bengaluru")]
            Bengaluru
        };



        public enum ApplicationSource
        {
            WebApp, Notification
        };

        public enum ErrorRedirectType
        {
            [Description("You are not permitted to access this portal as you are not a USI practitioner")]
            NonUsiPractitioner,

            [Description("You are unauthorized to view this page")]
            Unauthorized,

            [Description("Hello Practitioner! Please access the <a href='https://www.freelancer.com/deloitte/'>MyGigs</a> site henceforth.")]
            MyGigsRedirect,

            [Description("An unknown error occurred while processing your request.")]
            Unknown
        }
    }
}
