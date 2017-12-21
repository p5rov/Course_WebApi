using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SecretSanta.Repository;
using SecretSanta.Repository.Interfaces;

namespace SecretSanta.WebApi.Controllers
{
    public class LoginsController : ApiController
    {
        private IUserRepository m_UserRepository;
        public LoginsController()
        {
            m_UserRepository = new UserRepository();
        }

        // POST api/logins
        [AllowAnonymous]
        [Route("logins")]
        public HttpResponseMessage Post([FromBody] string userName, [FromBody] string password)
        {
            //#2
            // POST ~/ logins
            // Header: None
            // Body: 
            // {
            //     "username" : "...",
            //     "password" : "hash" }
            // Success:
            // 201.Created

            // Header: None
            // Body: { "authToken" : "..." }
            // Error:
            // 400 Bad request
            // 404 Not found
            // 401 Unauthorized - грешен username или парола
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "username and password are required");
            }

            IdentityResult result = await m_UserRepository.FindUser(userName, password);
            return Request.CreateResponse(HttpStatusCode.Created);
        }
        
        // DELETE api/logins/{username}
        public void Delete(string username)
        {
            //#3
            //DELETE ~/ logins /{ username}
            //Header: { "authToken" : "token" }
            //Body: None
            //    Success
            //204 No content

            //Header: None
            //Body: None
            //    Error
            //400 Bad request
            //404 Not found -вече отписан потребител
        }
    }
}