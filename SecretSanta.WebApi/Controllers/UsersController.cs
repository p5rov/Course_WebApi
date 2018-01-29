using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using SecretSanta.Repository;
using SecretSanta.Repository.Interfaces;
using SecretSanta.WebApi.Models;

namespace SecretSanta.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserRepository m_UserRepository;

        public UsersController()
        {
            m_UserRepository = new UserRepository();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_UserRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        public UserDto Get(string username)
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
            return new UserDto();
        }
        
        public List<UserDto> Get(int? skip, int? take, string order, string search)
        {
            //#4
            //GET ~/users?skip={s}&take={t}&order={Asc|Desc}&search={phrase}
            //Header : {"authToken" : "token" }
            //Success:
            //200 OK.
            //	Header: None
            //	Body: [ {"username" : "..." }, {...} ] 
            //Error:
            //400 bad request

            return new List<UserDto>();
        }
        
        [ResponseType(typeof(string))]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post([FromBody]UserDto userDto)
        {
            //#1
            //POST ~/ users
            //Header: None
            //Body:
            //{
            //    "username" : "...",
            //    "displayName" : "...",
            //    "password" : "hash" }
            //Success:
            //201.Created

            //Header: None
            //Body: { "displayName" : "..." }
            //Error:
            //400 Bad request
            //409 Conflict - неуникално име

            if (!ModelState.IsValid)
            {
                Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            IdentityResult result = await m_UserRepository.RegisterUser(userDto);
            HttpResponseMessage errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Request.CreateResponse(HttpStatusCode.Created, userDto.DisplayName);
        }

        [Route("users/{username}/invitations")]
        public HttpResponseMessage Post(string username, [FromBody]UserInvitationDto invitation)
        {
            //#7
            //POST ~/usrs/{username}/invitations
            //Header : {"authToken" : "..." }
            //Body: 
            //{"groupName" : "...",
            // "date" : "...",
            // "adminName" : "..." }
            //Success:
            //201 Created
            //	Header : None
            //	Body : {"id": "..."}
            //Error:
            //400 bad request
            //403 Forbiden  - покана за група, на която не е администратор
            //404 Not found - не съществува потребител/група
            //409 Conflict - потребителя има покана
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [Route("users/{username}/invitations")]
        [ResponseType(typeof(List<UserInvitationDto>))]
        public HttpResponseMessage GetInvitations(string username, int? skip, int? take, string order)
        {
            //#8
            //GET ~/users/{username}/invitations?skip={s}&take={t}&order={A|D} 
            //Header : {"authToken" : "..." }
            //Success:
            //200 Ok.
            //Header: None
            //Body: [{"groupName" : "...", "date" : "...", "adminName" : "..." }, ...]
            //Error:
            //400 Bad request
            //403 Forbiden - достъпване на чужди покани
            return Request.CreateResponse(HttpStatusCode.OK, new List<UserInvitationDto>());
        }

        [Route("users/{username}/invitations/{id}")]
        public HttpResponseMessage DeleteInvitation(string username, int id)
        {
            //#9c
            //DELETE ~/users/{username}/invitations/{id}
            //Header : {"authToken" : "..." }
            //Body : None
            //Success: 
            //204 No content
            //	Header: None
            //	Body: None
            //Error:
            //400 Bad request
            //409 Not found - изтрива липсваща покана
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        [Route("users/{username}/groups")]
        [ResponseType(typeof(List<GroupDto>))]
        public HttpResponseMessage GetUserGroups(string username, int? skip, int? take)
        {
            //#11
            //GET ~/users/{username}/groups?skip={s}&take={t}
            //Header : {"authToken" : "..." }
            //Success:
            //200 Ok.
            //	Header: none
            //	Body: [ ... ]
            //Error
            // 400 Bad request
            return Request.CreateResponse(HttpStatusCode.OK, new List<GroupDto>());
        }

        [Route("users/{username}/groups/{groupname}/links")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage GetLink(string username, string groupname)
        {
            //#12
            //GET ~users/{username}/groups/{groupname}/links
            //Header: {"authToken" : "..." }
            //Success: 
            // 200 OK
            //	Header : None 
            //	Body : {"reciever" : "username" } 
            // Error:
            // 400 bad request
            // 404 Not found - процесът още не е стартиран от създателя на групата ( точка б)
            return Request.CreateResponse(HttpStatusCode.Created, string.Empty);
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
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return null;
        }
    }
}