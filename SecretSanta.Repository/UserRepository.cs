using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecretSanta.Repository.Interfaces;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository
{
    public class UserRepository : IUserRepository
    {
        private AppContext m_AuthContext;
        private UserManager<IdentityUser> m_UserManager;

        public UserRepository()
        {
            m_AuthContext = new AppContext();
            m_UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(m_AuthContext));
        }

        public async Task<IdentityResult> RegisterUser(UserDto userDto)
        {
            IdentityUser user = new IdentityUser(userDto.UserName);
            IdentityResult result = await m_UserManager.CreateAsync(user, userDto.Password);
            return result;
        }

        public async Task<IdentityUser> FindUser(string username, string password)
        {
            IdentityUser user = await m_UserManager.FindAsync(username, password);
            return user;
        }

        public UserDto Get(string userDtoUserName)
        {
            throw new NotImplementedException();
        }

        public void Add(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            m_AuthContext.Dispose();
            m_UserManager.Dispose();
        }
    }
}
