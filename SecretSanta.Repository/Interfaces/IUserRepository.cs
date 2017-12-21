using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        UserDto Get(string userDtoUserName);
        void Add(UserDto userDto);
        Task<IdentityResult> RegisterUser(UserDto userDto);
        Task<IdentityUser> FindUser(string username, string password);
    }
}
