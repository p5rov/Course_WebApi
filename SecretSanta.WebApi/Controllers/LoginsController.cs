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
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNet.Identity.EntityFramework;

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
        public async Task<HttpResponseMessage> Post([FromBody] string userName, [FromBody] string password)
        {
            HttpRequest request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/api/token";
            using (HttpClient httpClient = new HttpClient())
            {
                IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>()
                                                                        {
                                                                            new KeyValuePair<string, string>("grant_type", "password"),
                                                                            new KeyValuePair<string, string>("username", userName),
                                                                            new KeyValuePair<string, string>("password", password),
                                                                        };
                FormUrlEncodedContent encodedParams = new FormUrlEncodedContent(requestParams);
                HttpResponseMessage responce = await httpClient.PostAsync(tokenServiceUrl, encodedParams);
                return responce;
            }
            // #2
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

            IdentityUser result = await m_UserRepository.FindUser(userName, password);
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