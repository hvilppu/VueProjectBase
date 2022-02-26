using Project.Domain.Models;
using System.Collections.Generic;

namespace Project.Domain.Dtos
{
    public class UserInfoDto : BaseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public override T FillFrom<T>(object source) where T : class
        {
            Users userSource = source as Users;

            if (userSource != null)
            {
                PropertyCopier<Users, UserInfoDto>.CopyProperties(userSource, this);
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
                PropertyCopier<UserInfoDto, Users>.CopyProperties(this, user);
                toReturn = user;
            }

            return (T)toReturn;
        }
    }
}
