using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
    public class PortfolioOffering
    {
        public int PortfolioId { get; set; }
        public int OfferingId { get; set; }
        public string DisplayName { get; set; }
        public string OfferingCode { get; set; }

    }
}
