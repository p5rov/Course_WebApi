using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SecretSanta.Repository;
using SecretSanta.Repository.Interfaces;
using SecretSanta.WebApi.Models;

namespace SecretSanta.WebApi.Controllers
{
    public class GroupsController : ApiController
    {
        private readonly IGroupRepository m_GroupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            m_GroupRepository = groupRepository;
        }

        // POST api/<controller>
        public GroupDto Post([FromBody] string groupName)
        {
            // #6
            // POST ~/groups 
            // Header : {"authToken" : "..." } 
            // Body : {"groupName" : "..."}
            // Success:
            // 201 Created.
            // Header: None
            // Body: { "groupName" : "..." , "adminName": "..."}
            // Error:
            // 400 bad request
            // 409 content - името не е уникално
            return new GroupDto();
        }

        [Route("groups/{groupName}/participants")]
        public HttpResponseMessage PostParticipants(string groupName, [FromBody]string userName)
        {
            // #9a
            // POST ~/groups/{groupName}/participants
            // Header : {"authToken" : "..." }
            // Body : {"username" : "..." }
            // Success: 
            // 200 Created
            // Error :
            // 400 Bad request
            // 403 Forbiden - нямаме покана за тази група
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("~/groups/{groupName}/links")]
        public HttpResponseMessage PostLinks(string groupName)
        {
            // #10
            // POST(или пък PUT) ~/groups/{groupname}/links
            // Header : {"authToken" : "..." }
            // Body : No

            // Success: 
            // 201. Created no body no headers
            // Error: 
            // 400 bad request 
            // 404 Not found - няма потребител в групата с username подаден в body-то на request-a
            // 403 Forbiden - не сме създател на въпросната група, за която правим свързване
            // 412 Precondition Failed - се опитаме да стартираме процес с един член ( точка е)
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("api/groups/{groupname}/participants")]
        [ResponseType(typeof(string[]))]
        public HttpResponseMessage GetGroupParticipants(string groupname)
        {
            // #13
            // GET ~/groups/{groupname}/participants
            // Header: {"authToken" : "..." }
            // Success:
            // 200 Ok
            // Header : None 
            // Body : [...]
            // Error:
            // 400 Bad request
            // 403 Forbiden - побителят не е администратор
            List<string> participants = m_GroupRepository.GetParticipants(groupname);
            return Request.CreateResponse(HttpStatusCode.OK, participants);
        }

        [Route("groups/{groupName}/participants/{participantUsername}")]
        public HttpResponseMessage DeleteParticipant(string groupName, string participantUsername)
        {
            // #14
            // DELETE ~/groups/{groupName}/participants/{participantUsername}
            // Header: {"authToken" : "..." }
            // Body: None
            // Success:
            // 204 No content
            // Header: None
            // Body: None
            // Error:
            // 400 Bad request
            // 404 Not found - няма такъв участник
            // 403 Forbiden - побителят не е администратор
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}