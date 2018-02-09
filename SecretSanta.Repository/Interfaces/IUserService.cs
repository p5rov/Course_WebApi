using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretSanta.WebApi.Models;

namespace SecretSanta.Repository.Interfaces
{
    public interface IUserService
    {
        UserDto GetByUserName(string username);

        List<UserDto> GetList(int? skip, int? take, string order, string search);
    }
}
