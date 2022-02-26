
using Project.Domain.Models;
using System.Collections.Generic;

namespace Project.Domain.Repositories
{
    public interface IUserRepository : IRepository
    {
        public Users GetUser(string uname, string pwd);
        List<Permissions> GetUserPermissions(int id);
        List<Roles> GetUserRoles(int id);
        void SetNewPassword(string newPassword);
    }
}