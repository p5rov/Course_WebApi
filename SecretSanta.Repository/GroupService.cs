using SecretSanta.Repository.Dto;
using SecretSanta.Repository.Interfaces;
using SecretSanta.Repository.Mappers;
using SecretSanta.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<GroupParticipant> m_GroupParticipantRepository;
        private readonly IRepository<Group> m_GroupRepository;
        private readonly IRepository<UserIdentity> m_UserRepository;
        private readonly IRepository<UserInvitation> m_UserInvitationRepository;
        public GroupService(IRepository<GroupParticipant> groupParticipantRepository,IRepository<UserInvitation> invitationRepository, IRepository<Group> groupRepository, IRepository<UserIdentity> userRepository)
        {
            m_GroupParticipantRepository = groupParticipantRepository;
            m_GroupRepository = groupRepository;
            m_UserRepository = userRepository;
            m_UserInvitationRepository = invitationRepository;
        }

        public void AddParticipant(string groupName, string username)
        {
            Group group = m_GroupRepository.Set().Where(p => p.Name == groupName).FirstOrDefault();
            UserIdentity user = m_UserRepository.Set().Where(p => p.UserName == username).FirstOrDefault();
            if (group != null && user != null)
            {
                GroupParticipant participant = new GroupParticipant
                {
                    Group = group,
                    User = user,
                    Fellow = null
                };

                m_GroupParticipantRepository.Set().Add(participant);
                m_GroupParticipantRepository.SaveChanges();
            }

            UserInvitation invitation = m_UserInvitationRepository.Set().Where(p => p.Group.Name == groupName && p.User.UserName == username).FirstOrDefault();
            if (invitation != null)
            {
                m_UserInvitationRepository.Set().Remove(invitation);
                m_UserInvitationRepository.SaveChanges();
            }
        }

        public GroupDto CreateGroup(string adminUserName, string groupName)
        {
            UserIdentity adminUser = m_UserRepository.Set().Where(p => p.UserName == adminUserName).FirstOrDefault();

            Group group = new Group();
            group.GroupAdminUser = adminUser;
            group.Name = groupName;
            m_GroupRepository.Set().Add(group);
            m_GroupRepository.SaveChanges();

            AddParticipant(groupName, adminUserName);
            return GroupMapper.MapToDto(group);
        }

        public void DeleteParticipant(string participantUsername, string groupName)
        {
            GroupParticipant participant = m_GroupParticipantRepository.Set().Where(p => p.Group.Name == groupName && p.User.UserName == participantUsername).FirstOrDefault();
            m_GroupParticipantRepository.Set().Remove(participant);
            m_GroupParticipantRepository.SaveChanges();
        }

        public GroupDto GetByName(string groupName)
        {
            Group group = m_GroupRepository.Set().Where(p => p.Name == groupName).FirstOrDefault();
            return group == null ? null : GroupMapper.MapToDto(group);
        }

        public string GetFellow(string username, string groupname)
        {
            GroupParticipant groupParticipant = m_GroupParticipantRepository.Set().Where(p => p.Group.Name == groupname && p.User.UserName == username).FirstOrDefault();
            return groupParticipant.Fellow.User.UserName;
        }

        public List<GroupDto> GetGroupsForUser(string username, int? skip = null, int? take = null)
        {
            var query = m_GroupParticipantRepository.Set().Where(p => p.User.UserName == username);
            if (skip > 0)
            {
                query = query.Skip(skip.Value);
            }

            if (take > 0)
            {
                query = query.Take(take.Value);
            }

            List<Group> groupList = query.Select(p => p.Group).ToList();
            return groupList.Select(GroupMapper.MapToDto).ToList();
        }

        public List<string> GetParticipants(string groupName)
        {
            Group group = m_GroupRepository.Set().Where(p => p.Name == groupName).FirstOrDefault();
            if (group != null)
            {
                return m_GroupParticipantRepository.Set().Where(p => p.GroupId == group.Id).Select(p => p.User.UserName).ToList();
            }

            return new List<string>();
        }

        public int GetParticipantsCount(string groupName)
        {
            return m_GroupParticipantRepository.Set().Where(p => p.Group.Name == groupName).Count();
        }

        public bool IsUserInGroup(string participantUsername, string groupName)
        {
            return m_GroupParticipantRepository.Set().Where(p => p.Group.Name == groupName && p.User.UserName == participantUsername).Count() > 0;
        }

        public void ProcessGroup(string groupName)
        {
            List<GroupParticipant> groupParticipants = m_GroupParticipantRepository.Set().Where(p => p.Group.Name == groupName).ToList();
            int count = groupParticipants.Count();
            List<int> indexList = new List<int>();
            
            for (int i =0; i < count; i++)
            {
                indexList.Add(i);
            }

            Random random = new Random();
            for (int i = count - 1; i >= 1; i--)
            {
                int fellowIndex = random.Next(0, i);
                int item = indexList[fellowIndex];
                if (item == i)
                {
                    i++;
                    continue;
                }

                indexList.Remove(item);
                groupParticipants[i].Fellow = groupParticipants[item];
            }

            if (indexList[0] == 0)
            {
                int nextRandom = random.Next(1, count - 1);
                groupParticipants[0].Fellow = groupParticipants[nextRandom].Fellow;
                groupParticipants[nextRandom].Fellow = groupParticipants[0];
            }
            else
            {
                groupParticipants[0].Fellow = groupParticipants[indexList[0]];
            }

            m_GroupParticipantRepository.SaveChanges();
        }
    }
}
