using DCode.Models.Base;
using System;

namespace DCode.Models.ResponseModels.Task
{
    public class Task : TaskSummary, IViewModel
    {
        public int Id { get; set; }
        public string ProjectWBSCode { get; set; }
        public int TypeId { get; set; }
        public string Details { get; set; }
        public string RequestorEmailId { get; set; }
        public string Skills { get; set; }
        public string Comments { get; set; }
        public string OnBoardingDate { get; set; }
        public string ServiceLine { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public System.DateTime? StatusDate { get; set; }
        public string Duration { get; set; }
        public bool IsApplied { get; set; }
        public bool IsRecommended { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public string UpdatedBy
        {
            get;
            set;
        }

        public DateTime? UpdatedOn
        {
            get;
            set;
        }
    }
}
