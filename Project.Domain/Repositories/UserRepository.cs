using project.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project.Domain.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {

        public UserRepository(ProjectContext projectContext, IHttpContextAccessor httpContextAccessor) : base(projectContext, httpContextAccessor)
        {

        }

        public Users GetUser(string userName, string password)
        {
            var saltedPassword = Helpers.HashPassword(password);

            var user = this._dbContext.Users
                .Where(u => u.UserName.ToUpper() == userName.ToUpper() && u.Password == saltedPassword)
                .FirstOrDefault();

            if (user == null)
                return null;

            return user;
        }

        public List<Permissions> GetUserPermissions(int id)
        {
            var permissions = new List<Permissions>();

            var roles = GetUserRoles(id);

            foreach (var role in roles)
            {
                var rolePermissions = this._dbContext.RolePermissions.Where(rp => rp.RoleId == role.Id).ToList();

                foreach (var rp in rolePermissions)
                {
                    var permission = this._dbContext.Permissions.Where(p => p.Id == rp.PermissionId).FirstOrDefault();

                    var alreadyGot = (from p in permissions where p.Code == permission.Code select p).FirstOrDefault();

                    if (alreadyGot == null)
                        permissions.Add(permission);
                }
            }

            return permissions;
        }

        public List<Models.Roles> GetUserRoles(int id)
        {
            var roles = new List<Models.Roles>();

            var userInRoles = this._dbContext.UserRoles.Where(ur => ur.UserId == id).ToList();

            foreach (var uir in userInRoles)
            {
                var role = this._dbContext.Roles.Where(r => r.Id == uir.RoleId).FirstOrDefault();

                var alreadyGot = (from r in roles where r.Code == role.Code select r).FirstOrDefault();

                if (alreadyGot == null)
                    roles.Add(role);
            }

            return roles;
        }    

        public void SetNewPassword(string newPassword)
        {
            currentUser.Password = Helpers.HashPassword(newPassword);
            currentUser.PasswordSet = true;
            Update<Users>(currentUser);
        }
    }
}