using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SecretSanta.Repository;
using SecretSanta.Repository.Dto;
using SecretSanta.Repository.Interfaces;
using SecretSanta.WebApi.AuthorizationAttributes;
using SecretSanta.WebApi.Models;

namespace SecretSanta.WebApi.Controllers
{
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService m_GroupService;
        private readonly IUserService m_UserService;

        public GroupsController(IGroupService groupService, IUserService userService)
        {
            m_GroupService = groupService;
            m_UserService = userService;
        }

        // POST api/<controller>;2
        [Route("api/groups")]
        [ResponseType(typeof(GroupDto))]
        [UserAuthorize]
        public HttpResponseMessage Post([FromBody]string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            GroupDto group = m_GroupService.GetByName(groupName);
            if (group != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "group with same name already exists");
            }

            GroupDto createdGroup = m_GroupService.CreateGroup(GetUserName(), groupName);
            return Request.CreateResponse(HttpStatusCode.Created, createdGroup);
        }

        [Route("api/groups/{groupName}/participants")]
        [UserAuthorize]
        public HttpResponseMessage PostParticipants(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (!m_UserService.IsInvited(GetUserName(), groupName))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "user not invited to this group");
            }
            
            // add participant delete invitation;
            m_GroupService.AddParticipant(groupName, GetUserName());
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("api/groups/{groupName}/links")]
        [UserAuthorize]
        public HttpResponseMessage PostLinks(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            GroupDto group = m_GroupService.GetByName(groupName);
            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "group not found");
            }

            if (group.AdminName != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "user not admin for the group");
            }

            int participantsCount = m_GroupService.GetParticipantsCount(groupName);
            if (participantsCount <= 1)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "not enough participants");
            }

            m_GroupService.ProcessGroup(groupName);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("api/groups/{groupname}/participants")]
        [ResponseType(typeof(List<string>))]
        [UserAuthorize]
        public HttpResponseMessage GetGroupParticipants(string groupname)
        {
            if (string.IsNullOrEmpty(groupname))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            GroupDto group = m_GroupService.GetByName(groupname);
            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "group not found");
            }

            if (group.AdminName != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "user not admin for the group");
            }

            List<string> participants = m_GroupService.GetParticipants(groupname);
            return Request.CreateResponse(HttpStatusCode.OK, participants);
        }

        [Route("groups/{groupName}/participants/{participantUsername}")]
        [UserAuthorize]
        public HttpResponseMessage DeleteParticipant(string groupName, string participantUsername)
        {
            // #14
            if (string.IsNullOrEmpty(groupName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            GroupDto group = m_GroupService.GetByName(groupName);
            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "group not found");
            }

            if (group.AdminName != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "user not admin for the group");
            }

            if (!m_GroupService.IsUserInGroup(participantUsername, groupName))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, string.Format("user {0} not in group {1}", participantUsername, groupName));
            }

            m_GroupService.DeleteParticipant(participantUsername, groupName);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}