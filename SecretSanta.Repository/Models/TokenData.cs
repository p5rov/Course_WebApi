using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Models
{
    public class TokenData
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Token { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
