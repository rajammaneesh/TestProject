using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
    public class Offering
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public int PortfolioId { get; set; }

        public string RMEmailGroup { get; set; }
        public string PracticeEmailGroup { get; set; }

        public List<string> GetRmEmailsAsList()
        {
            return SplitEmailsToList(RMEmailGroup);
        }

        public List<string> GetPracticeEmailGroupsAsList()
        {
            return SplitEmailsToList(PracticeEmailGroup);
        }

        private List<string> SplitEmailsToList(string emailString)
        {
            return emailString?.Split(';')?.ToList();
        }
    }
}
