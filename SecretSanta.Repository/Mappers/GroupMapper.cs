using SecretSanta.Repository.Dto;
using SecretSanta.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Mappers
{
    public class GroupMapper
    {
        public static GroupDto MapToDto(Group source)
        {
            GroupDto target = new GroupDto
            {
                AdminName = source.GroupAdminUser.UserName,
                GroupName = source.Name,
            };

            return target;
        }
    }
}
