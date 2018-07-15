using System.Collections.Generic;
using System.Linq;

namespace DCode.Models.ResponseModels.Common
{
    public class Offering
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Portfolio_Id { get; set; }
        public string Code { get; set; }
        public string RM_Email_Group { get; set; }
        public string Practice_Email_Group { get; set; }

        public List<string> GetRmEmailsAsList()
        {
            return SplitEmailsToList(RM_Email_Group);
        }

        public List<string> GetPracticeEmailGroupsAsList()
        {
            return SplitEmailsToList(Practice_Email_Group);
        }

        private List<string> SplitEmailsToList(string emailString)
        {
            return emailString?.Split(',')?.ToList();
        }
    }
}
