﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
    public class SubOffering
    {
        public int Id { get; set; }

        public int OfferingId { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
