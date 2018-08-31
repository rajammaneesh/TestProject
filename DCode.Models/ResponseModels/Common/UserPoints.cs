using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Common
{
    public class UserPoints
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public string @event { get; set; }
        public int points { get; set; }
        public System.DateTime created_date { get; set; }

    }
}
