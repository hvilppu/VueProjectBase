using Project.Domain.Models;
using System.Collections.Generic;
namespace Project.Domain.Dtos
{
    public class UserDto : BaseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public bool PasswordSet { get; set; }

        public List<string> InRoles { get; set; }
        public List<int> InFacilitys { get; set; }

        public bool Deleted { get; set; }

        public override T FillFrom<T>(object source) where T : class
        {
            Users userSource = source as Users;

            if (userSource != null)
            {
                PropertyCopier<Users, UserDto>.CopyProperties(userSource, this);

                this.InRoles = new List<string>();
                foreach (var role in userSource.InRoles)
                    this.InRoles.Add(role);

                this.InFacilitys = new List<int>();
                foreach (var facility in userSource.InFacilitys)
                    this.InFacilitys.Add(facility);

            }
            return (T)(object)this;
        }

        public override T FillTo<T>(T toFill) where T : class
        {

            object toReturn = null;
            Users user = toFill as Users;
            if (toFill == null)
                user = new Users();

            if (user != null)
            {
                PropertyCopier<UserDto, Users>.CopyProperties(this, user);

                if (this.InRoles != null)
                {
                    user.InRoles = new List<string>();

                    foreach (var role in this.InRoles)
                        user.InRoles.Add(role);
                }

                if (this.InFacilitys != null)
                {
                    user.InFacilitys = new List<int>();

                    foreach (var facility in this.InFacilitys)
                        user.InFacilitys.Add(facility);
                }

                toReturn = user;

            }

            return (T)toReturn;
        }
    }
}
