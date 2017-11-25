using System;

namespace DCode.Models.ResponseModels.Task
{
    public class TaskSummary
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string DueDate { get; set; }
        public int? Hours { get; set; }
        public string ShortName 
        {
            get
            {
                if(ProjectName.Trim().Contains(" "))
                {
                    var split = ProjectName.Split(' ');
                    return split[0].Substring(0,1) + string.Empty + split[1].Substring(0,1);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ProjectName) && ProjectName.Length > 2)
                    {
                        return ProjectName.Substring(0, 2);
                    }
                    else
                    {
                        return ProjectName;
                    }
                }
            }
        }
        //Do not add more properties, use instead Task model
    }
}
