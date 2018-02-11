using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSanta.WebApi.Dto;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository.Interfaces
{
    public interface IUserService
    {
        UserDto GetByUserName(string username);
        List<UserDto> GetList(int? skip, int? take, string order, string search);
        void Invite(string user, string group);
        bool IsInvited(string username, string groupName);
        List<UserInvitationDto> GetInvitations(string username, int? skip, int? take, string order);
        UserInvitationDto GetInvitation(int id);
        void DeleteInvitation(int id);
    }
}
