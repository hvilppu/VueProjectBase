using project.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Project.Domain.Models;
using Project.Common;

namespace Project.Tests
{
    public static class DbContext
    {
        public static ProjectContext MemoryContext()
        {
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<ProjectContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new ProjectContext(options);
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        internal static void SetUserData(ProjectContext context)
        {
            string saltedPassword = Helpers.HashPassword("rrrr!");

            var user = new Users()
            {
                Id = 1,
                UserName = "TestUser",
                FirstName = "Teppo",
                SecondName = "Testinen",
                Password = "zTl+pF+wQaw3VbGoV41B5AEkKqsLXtcFWpXTkCf+/Gs=" // hashed = 'passwd',
            };
            context.Users.Add(user);

            var role = new Roles()
            {
                Id = 1,
                Code = "Admin"
            };
            context.Roles.Add(role);

            var permission = new Permissions()
            {
                Id = 1,
                Code = "Common_Admin"
            };
            context.Permissions.Add(permission);

            var rolePermission = new RolePermissions()
            {
                RoleId = 1,
                PermissionId = 1
            };
            context.RolePermissions.Add(rolePermission);

            var role2 = new Roles()
            {
                Id = 2,
                Code = "SuperAdmin"
            };
            context.Roles.Add(role2);

            var permission2 = new Permissions()
            {
                Id = 2,
                Code = "Super_Admin"
            };
            context.Permissions.Add(permission2);

            var rolePermission2 = new RolePermissions()
            {
                RoleId = 2,
                PermissionId = 2
            };
            context.RolePermissions.Add(rolePermission2);

            var userRole = new UserRoles()
            {
                UserId = 1,
                RoleId = 1
            };
            context.UserRoles.Add(userRole);

            var user2 = new Users()
            {
                Id = 2,
                UserName = "TestUser2",
                FirstName = "TestUserFirstName",
                SecondName = "TestUserSecondName",
                Password = "AAAA" // Not working
            };
            context.Users.Add(user2);

            var userRole2 = new UserRoles()
            {
                UserId = 2,
                RoleId = 1
            };
            context.UserRoles.Add(userRole2);


            context.SaveChanges();
        }
    }
}
