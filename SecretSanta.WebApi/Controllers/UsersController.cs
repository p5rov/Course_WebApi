using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using SecretSanta.Repository;
using SecretSanta.Repository.Dto;
using SecretSanta.Repository.Interfaces;
using SecretSanta.Repository.Models;
using SecretSanta.WebApi.AuthorizationAttributes;
using SecretSanta.WebApi.Dto;
using SecretSanta.WebApi.Models;

namespace SecretSanta.WebApi.Controllers
{
    public class UsersController : ControllerBase
    {
        private IUserService m_UserService;
        private readonly IGroupService m_GroupService;

        public UsersController(IUserService userService, IGroupService groupService)
        {
            m_UserService = userService;
            m_GroupService = groupService;
        }

        [Route("api/users/{username}")]
        [ResponseType(typeof(UserDto))]
        [UserAuthorize]
        public HttpResponseMessage Get(string username)
        {
            //#5
            //GET ~/users/{username}
            //Header : {"authToken" : "token" } 
            //Success:
            //200 Ok.
            //	Header: None
            //	Body: {"username" : "..." , "displayName" : "..." } 
            //Error:
            //400 Bad request
            //404 Not found
            
            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            UserDto user = m_UserService.GetByUserName(username);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "user not found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }


        [Route("api/users")]
        [ResponseType(typeof(List<UserDto>))]
        [UserAuthorize]
        public HttpResponseMessage Get(int? skip = null, int? take = null, string order = null, string search = null)
        {
            if (skip <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid skip value");
            }

            if (take <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid take value");
            }

            if (!string.IsNullOrEmpty(order) && order.ToLower() != "asc" && order.ToLower() != "desc")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalud order value");
            }
            
            List<UserDto> userList = m_UserService.GetList(skip, take, order, search);
            return Request.CreateResponse(HttpStatusCode.OK, userList);
        }
        
        [Route("api/users")]
        [ResponseType(typeof(string))]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post([FromBody]UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            
            using (SecretSantaUserManager userManager = SecretSantaUserManager.GetInstance())
            {
                UserIdentity userIdentity = new UserIdentity { UserName = userDto.UserName, DisplayName = userDto.DisplayName, };
                IdentityResult result = await userManager.CreateAsync(userIdentity, userDto.Password);
                HttpResponseMessage errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
            }

            return Request.CreateResponse(HttpStatusCode.Created, userDto.DisplayName);
        }

        [Route("api/users/{username}/invitations")]
        [HttpPost]
        [UserAuthorize]
        public HttpResponseMessage PostInvitations(string username, [FromBody]UserInvitationDto invitation)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (username == GetUserName())
            {
                Request.CreateResponse(HttpStatusCode.BadRequest, "cannot invite yourself :)");
            }

            GroupDto group = m_GroupService.GetByName(invitation.GroupName);
            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "group not found.");
            }

            UserDto user = m_UserService.GetByUserName(username);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "user not found");
            }

            if (group.AdminName != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "you are not creator of the group.");
            }

            List<string> participants = m_GroupService.GetParticipants(group.GroupName);
            if (participants.Contains(username))
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "user already in group");
            }

            bool isInvited = m_UserService.IsInvited(username, invitation.GroupName);
            if (isInvited)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "user already invited to this group");
            }

            m_UserService.Invite(username, invitation.GroupName);
            
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [Route("api/users/{username}/invitations")]
        [ResponseType(typeof(List<UserInvitationDto>))]
        [UserAuthorize]
        public HttpResponseMessage GetInvitations(string username, int? skip = null, int? take = null , string order = null)
        {
            if (skip <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid skip value");
            }

            if (take <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid take value");
            }

            if (!string.IsNullOrEmpty(order) && order.ToLower() != "asc" && order.ToLower() != "desc")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalud order value");
            }

            if (username != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Cannot get invitations for different user");
            }

            List<UserInvitationDto> list = m_UserService.GetInvitations(username, skip, take, order);
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [Route("api/users/{username}/invitations/{id}")]
        [UserAuthorize]
        public HttpResponseMessage DeleteInvitation(string username, int id)
        {
            if (username != GetUserName() || id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            UserInvitationDto invitation = m_UserService.GetInvitation(id);
            if (invitation == null || invitation.Username != username)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "invitation not found");
            }

            m_UserService.DeleteInvitation(id);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        [Route("api/users/{username}/groups")]
        [ResponseType(typeof(List<GroupDto>))]
        [UserAuthorize]
        public HttpResponseMessage GetUserGroups(string username, int? skip = null, int? take = null)
        {
            if (skip <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid skip value");
            }

            if (take <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid take value");
            }

            if (username != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "not same user");
            }

            List<GroupDto> userGroups = m_GroupService.GetGroupsForUser(username);
            return Request.CreateResponse(HttpStatusCode.OK, userGroups);
        }

        [Route("api/users/{username}/groups/{groupname}/links")]
        [ResponseType(typeof(string))]
        [UserAuthorize]
        public HttpResponseMessage GetLink(string username, string groupname)
        {
            if (username != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "not same user");
            }

            string fellow = m_GroupService.GetFellow(username, groupname);
            if (string.IsNullOrEmpty(fellow))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, string.Format("process not started or user not in {0}", groupname));
            }

            return Request.CreateResponse(HttpStatusCode.Created, fellow);
        }

        private HttpResponseMessage GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return null;
        }
    }
}