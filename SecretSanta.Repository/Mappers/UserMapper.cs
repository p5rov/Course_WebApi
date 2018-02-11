using SecretSanta.Repository.Models;
using SecretSanta.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Mappers
{
    public class UserMapper
    {
        public static UserDto MapToUserDto(UserIdentity userIdentity)
        {
            UserDto user = new UserDto();
            user.DisplayName = userIdentity.DisplayName;
            user.UserName = userIdentity.UserName;
            return user;
        }
    }
}
