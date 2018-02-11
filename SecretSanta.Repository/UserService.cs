using SecretSanta.Repository.Interfaces;
using SecretSanta.Repository.Mappers;
using SecretSanta.Repository.Models;
using SecretSanta.WebApi.Dto;
using SecretSanta.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserIdentity> m_UserRepository;
        private readonly IRepository<UserInvitation> m_UserInvitationRepository;
        private readonly IRepository<Group> m_GroupRepository;

        public UserService(IRepository<UserIdentity> repository, IRepository<UserInvitation> userInvitationRepository, IRepository<Group> groupRepository)
        {
            m_UserRepository = repository;
            m_UserInvitationRepository = userInvitationRepository;
            m_GroupRepository = groupRepository;
        }

        public void DeleteInvitation(int id)
        {
            UserInvitation invitation = m_UserInvitationRepository.FindById(id);
            m_UserInvitationRepository.Set().Remove(invitation);
            m_UserInvitationRepository.SaveChanges();
        }

        public UserDto GetByUserName(string username)
        {
            UserIdentity user = m_UserRepository.Set().Where(p => p.UserName == username).FirstOrDefault();
            return user == null ? null : UserMapper.MapToUserDto(user);
        }

        public UserInvitationDto GetInvitation(int id)
        {
            UserInvitation invitation = m_UserInvitationRepository.FindById(id);
            return invitation == null ? null : UserInvitationMapper.MapToDto(invitation);
        }
        
        public List<UserInvitationDto> GetInvitations(string username, int? skip, int? take, string order)
        {
            var query = m_UserInvitationRepository.Set().Where(p => p.User.UserName == username);


            if (!string.IsNullOrEmpty(order) && order.ToLower() == "desc")
            {
                query = query.OrderByDescending(p => p.Date);
            }
            else
            {
                query = query.OrderBy(p => p.Date);
            }

            if (skip > 0)
            {
                query = query.Skip(skip.Value);
            }

            if (take > 0)
            {
                query = query.Take(take.Value);
            }

            return query.ToList().Select(UserInvitationMapper.MapToDto).ToList();
        }

        public List<UserDto> GetList(int? skip, int? take, string order, string search)
        {
            IQueryable<UserIdentity> query = m_UserRepository.Set();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.UserName.Contains(search) || p.DisplayName.Contains(search));
            }

            if (!string.IsNullOrEmpty(order))
            {
                if (order.ToLower() == "asc")
                {
                    query = query.OrderBy(p => p.DisplayName);
                }

                if (order.ToLower() == "desc")
                {
                    query = query.OrderByDescending(p => p.DisplayName);
                }
            }

            if (skip > 0)
            {
                query = query.Skip(skip.Value);
            }

            if (take > 0)
            {
                query = query.Take(take.Value);
            }

            return query.ToList().Select(p => UserMapper.MapToUserDto(p)).ToList();
        }

        public void Invite(string userName, string groupName)
        {
            UserIdentity user = m_UserRepository.Set().Where(p => p.UserName == userName).FirstOrDefault();
            Group group = m_GroupRepository.Set().Where(p => p.Name == groupName).FirstOrDefault();
            if (user != null && group != null)
            {

                UserInvitation target = new UserInvitation
                {
                    Date = DateTime.Now,
                    User = user,
                    Group = group
                };

                m_UserInvitationRepository.Set().Add(target);
                m_UserInvitationRepository.SaveChanges();
            }
        }

        public bool IsInvited(string username, string groupName)
        {
            return m_UserInvitationRepository.Set().Where(p => p.Group.Name == groupName && p.User.UserName == username).Count() > 0;
        }
    }
}
