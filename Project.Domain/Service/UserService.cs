using Project.Domain.Models;
using Project.Domain.Repositories;
using System.Collections.Generic;

namespace Project.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Users GetUser(string username, string password)
        {
            return _userRepository.GetUser(username, password);
        }

        public List<Permissions> GetUserPermissions(int id)
        {
            return _userRepository.GetUserPermissions(id);
        }

        public List<Roles> GetUserRoles(int id)
        {
            return _userRepository.GetUserRoles(id);
        }
    }
}