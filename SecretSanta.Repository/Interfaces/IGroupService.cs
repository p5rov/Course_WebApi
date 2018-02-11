using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSanta.Repository.Dto;

namespace SecretSanta.Repository.Interfaces
{
    public interface IGroupService
    {
        GroupDto GetByName(string groupName);
        List<string> GetParticipants(string groupName);
        List<GroupDto> GetGroupsForUser(string username, int? skip = null, int? take = null);
        void AddParticipant(string groupName, string username);
        string GetFellow(string username, string groupname);
        GroupDto CreateGroup(string v, string groupName);
        void ProcessGroup(string groupName);
        int GetParticipantsCount(string groupName);
        bool IsUserInGroup(string participantUsername, string groupName);
        void DeleteParticipant(string participantUsername, string groupName);
    }
}
