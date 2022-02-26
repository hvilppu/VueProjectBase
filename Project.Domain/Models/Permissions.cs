using System;
using System.Collections.Generic;

namespace Project.Domain.Models
{
    public partial class Permissions
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime? LastModified { get; set; }
        public string Modifier { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
