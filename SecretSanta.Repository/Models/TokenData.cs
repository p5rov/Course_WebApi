using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Models
{
    public class TokenData
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
