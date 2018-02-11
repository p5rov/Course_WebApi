using SecretSanta.Repository.Models;
using SecretSanta.WebApi.Dto;
using SecretSanta.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Mappers
{
    public class UserInvitationMapper
    {
        public static UserInvitationDto MapToDto(UserInvitation source)
        {
            UserInvitationDto target = new UserInvitationDto();
            target.Id = source.Id;
            target.Date = source.Date;
            target.GroupName = source.Group.Name;
            target.Username = source.User.UserName;
            return target;
        }
    }
}
