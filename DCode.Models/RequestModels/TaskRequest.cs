using System.Collections.Generic;
using static DCode.Models.Enums.Enums;

namespace DCode.Models.RequestModels
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public string ProjectName { get; set; }
        public List<int> SkillSet { get; set; }
        public int Hours { get; set; }
        public string OnBoardingDate { get; set; }
        public string DueDate { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool GiftsOrAwards { get; set; }
        public string Comments { get; set; }
        public string Details { get; set; }
        public string RequestorEmailId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ProjectWBSCode { get; set; }
        public int TaskType { get; set; }

        public string SelectedServiceLine { get; set; }
    }
}
