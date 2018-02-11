using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretSanta.Repository.Interfaces;
using SecretSanta.Repository.Models;

namespace SecretSanta.Repository
{
    public class TokenService : ITokenService
    {
        private IRepository<TokenData> m_TokenDataRepository;

        public TokenService(IRepository<TokenData> tokenDataRepository)
        {
            m_TokenDataRepository = tokenDataRepository;
        }

        public void SaveToken(string userName, string token)
        {
            TokenData tokenData = new TokenData();
            tokenData.UserName = userName;
            tokenData.CreateDate = DateTime.Now;
            tokenData.Token = token;
            tokenData.ExpirationDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationPeriod"]));
            m_TokenDataRepository.Set().Add(tokenData);
            m_TokenDataRepository.SaveChanges();
        }

        public bool IsValidToken(string userName, string token)
        {
            TokenData tokenData = m_TokenDataRepository.Set().FirstOrDefault(p => p.UserName == userName && p.Token == token);
            if (tokenData == null)
            {
                return false;
            }

            if (tokenData.ExpirationDate < DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public void ExtendTokenPeriod(string userName, string token)
        {
            TokenData tokenData = m_TokenDataRepository.Set().FirstOrDefault(p => p.UserName == userName && p.Token == token);
            if (tokenData != null)
            {
                tokenData.ExpirationDate = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationPeriod"]));
                m_TokenDataRepository.SaveChanges();
            }
        }

        public void DeleteTokens(string userName)
        {
            TokenData[] tokens = m_TokenDataRepository.Set().Where(p => p.UserName == userName).ToArray();
            foreach (TokenData token in tokens)
            {
                m_TokenDataRepository.Set().Remove(token);
            }

            m_TokenDataRepository.SaveChanges();
        }
    }
}
