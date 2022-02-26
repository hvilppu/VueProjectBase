using System;
using System.Collections.Generic;

namespace Project.Domain.Models
{
    public partial class Users
    {
        public Users()
        {

        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool PasswordSet { get; set; }
        public string Password { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime? LastModified { get; set; }
        public string Modifier { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
