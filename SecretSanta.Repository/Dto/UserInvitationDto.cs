using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.WebApi.Dto
{
    public class UserInvitationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string GroupName { get; set; }
        public string Username { get; set; }
    }
}