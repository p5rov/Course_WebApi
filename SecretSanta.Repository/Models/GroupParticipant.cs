using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Models
{
    public class GroupParticipant
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual UserIdentity User { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

    }
}
