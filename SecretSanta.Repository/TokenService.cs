using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretSanta.Repository.Models;

namespace SecretSanta.Repository
{
    public class TokenService
    {
        public void SaveToken(string userName, string token)
        {
            TokenData tokenData = new TokenData();
            tokenData.UserName = userName;
            tokenData.Token = token;
            tokenData.ExpirationDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationPeriod"]));
            using (SecretSantaContext context = new SecretSantaContext())
            {
                context.TokenData.Add(tokenData);
            }
        }

        public bool IsValidToken(string userName, string token)
        {
            using (SecretSantaContext context = new SecretSantaContext())
            {
                TokenData tokenData = context.TokenData.Where(p => p.UserName == userName && p.Token == token).FirstOrDefault();
                if (tokenData == null)
                {
                    return false;
                }

                if (tokenData.ExpirationDate < DateTime.Now)
                {
                    return false;
                }
            }

            return true;
        }

        public void ExtendTokenPeriod(string userName, string token)
        {
            using (SecretSantaContext context = new SecretSantaContext())
            {
                TokenData tokenData = context.TokenData.Where(p => p.UserName == userName && p.Token == token).FirstOrDefault();
                if (tokenData != null)
                {
                    tokenData.ExpirationDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationPeriod"]));
                    context.SaveChanges();
                }
            }
        }
    }
}
