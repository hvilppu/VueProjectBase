using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Models
{
    public partial class Users : IUpdateable<Users>
    {
        [NotMapped]
        public bool IsDirty { get; set; }
        [NotMapped]
        public List<string> InRoles = new List<string>();
        [NotMapped]
        public List<int> InFacilitys = new List<int>();
        public void Update(Users source)
        {
            throw new System.NotImplementedException();
        }
    }
}
