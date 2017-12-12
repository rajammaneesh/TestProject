using System.Collections.Generic;

namespace DCode.Models.ResponseModels.Requestor
{
    public class PermissionTaskResponse
    {
        public IEnumerable<PermissionsTask> permissionTasks { get; set; }
        public int TotalRecords { get; set; }
    }
}