using System;

namespace Project.Domain
{
    public interface IModel
    {
        DateTime? CreationDate { get; set; }
        bool? Active { get; set; }
        bool Deleted { get; set; }
        DateTime? LastModified { get; set; }
        string Modifier { get; set; }
        DateTime? ValidUntil { get; set; }
    }
}
