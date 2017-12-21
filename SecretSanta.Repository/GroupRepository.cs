using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSanta.Repository.Interfaces;

namespace SecretSanta.Repository
{
    public class GroupRepository : IGroupRepository
    {
        public List<string> GetParticipants(string groupName)
        {
            throw new NotImplementedException();
        }

        public void DeleteParticipant(string groupName, string participantUsername)
        {
            throw new NotImplementedException();
        }

        public void AddParticipant(string groupName, string participantUserName)
        {
            throw new NotImplementedException();
        }

        public void Add(string groupName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetForUser(string username, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLinks(string username, string groupName)
        {
            throw new NotImplementedException();
        }
    }
}
