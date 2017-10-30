using DCode.Models.ResponseModels.Requestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Models.ResponseModels.Requestor
{
    public class PermissionTaskResponse
    {
        public IEnumerable<PermissionsTask> permissionTasks { get; set; }
        public int TotalRecords { get; set; }
    }
}