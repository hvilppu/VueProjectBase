using System;
using System.Collections.Generic;

namespace Project.Domain.Models
{
    public partial class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
