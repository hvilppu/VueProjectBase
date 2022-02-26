using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Domain.Repositories;
using Project.Domain.Services;
using Project.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Tests.Domain.Services
{
    [TestClass]
    public class UserServiceTests
    {
        public UserServiceTests()
        {
        }

        [TestMethod]
        public void GetUser()
        {
            using var context = DbContext.MemoryContext();
            DbContext.SetUserData(context);
            using UserRepository userRepository = new UserRepository(context, null);
            UserService UserService = new UserService(userRepository);
            var user = UserService.GetUser("TestUser", "passwd");
            Assert.IsTrue(user != null);
        }

        [TestMethod]
        public void GetUserPermissions()
        {
            using var context = DbContext.MemoryContext();
            DbContext.SetUserData(context);
            using UserRepository userRepository = new UserRepository(context, null);
            UserService UserService = new UserService(userRepository);
            var userPermissions = UserService.GetUserPermissions(1);
            Assert.IsTrue(userPermissions != null && userPermissions.Count == 1 && userPermissions.FirstOrDefault().Code == "Common_Admin");
        }

        [TestMethod]
        public void GetUserRoles()
        {
            using var context = DbContext.MemoryContext();
            DbContext.SetUserData(context);
            using UserRepository userRepository = new UserRepository(context, null);
            UserService UserService = new UserService(userRepository);
            var userRoles = UserService.GetUserRoles(1);
            Assert.IsTrue(userRoles != null && userRoles.Count == 1 && userRoles.FirstOrDefault().Code == "Admin");
        }
    }
}
