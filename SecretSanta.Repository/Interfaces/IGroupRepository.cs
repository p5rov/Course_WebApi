using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Interfaces
{
    public interface IGroupRepository
    {
        List<string> GetParticipants(string groupName);
        void DeleteParticipant(string groupName, string participantUsername);
        void AddParticipant(string groupName, string participantUserName);
        void Add(string groupName);
        List<string> GetForUser(string username, int? skip, int? take);
        List<string> GetLinks(string username, string groupName);
    }
}
