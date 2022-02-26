using Project.Domain.Models;
using System.Collections.Generic;

namespace Project.Domain.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Get user with username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Users GetUser(string username, string password);

        List<Permissions> GetUserPermissions(int id);
        List<Roles> GetUserRoles(int id);
    }
}