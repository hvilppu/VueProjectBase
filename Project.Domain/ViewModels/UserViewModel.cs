using System.Collections.Generic;

namespace Project.Domain.ViewModels
{
    public class UserViewModel
    {
        public string name { get; set; }
        public List<string> permissions { get; set; }
        public bool needSetPassword { get; set; }
    }
}
