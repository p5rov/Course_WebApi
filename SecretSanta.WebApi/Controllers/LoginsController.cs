using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

using Microsoft.AspNet.Identity;

using Newtonsoft.Json;

using SecretSanta.Repository;
using SecretSanta.Repository.Dto;
using SecretSanta.Repository.Interfaces;
using SecretSanta.Repository.Models;
using SecretSanta.WebApi.AuthorizationAttributes;

namespace SecretSanta.WebApi.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class LoginsController : ControllerBase
    {
        private ITokenService m_TokenService;

        public LoginsController(ITokenService tokenService)
        {
            m_TokenService = tokenService;
        }

        // POST api/logins
        [Route("api/logins")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            HttpRequest request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "token";
            using (HttpClient httpClient = new HttpClient())
            {
                IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>()
                                                                        {
                                                                            new KeyValuePair<string, string>("grant_type", "password"),
                                                                            new KeyValuePair<string, string>("username", login.UserName),
                                                                            new KeyValuePair<string, string>("password", login.Password),
                                                                        };
                FormUrlEncodedContent encodedParams = new FormUrlEncodedContent(requestParams);
                HttpResponseMessage responce = await httpClient.PostAsync(tokenServiceUrl, encodedParams);
                if (responce.StatusCode == HttpStatusCode.OK)
                {
                    string responceMessage = await responce.Content.ReadAsStringAsync();
                    Dictionary<string, string> jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responceMessage);
                    var authToken = jsonData["access_token"];
                    m_TokenService.SaveToken(login.UserName, authToken);
                }

                return responce;
            }
        }

        // DELETE api/logins/{username}
        [Route("api/logins/{username}")]
        [UserAuthorize]
        public HttpResponseMessage Delete(string username)
        {
            if (username != GetUserName())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");
            }

            m_TokenService.DeleteTokens(username);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}