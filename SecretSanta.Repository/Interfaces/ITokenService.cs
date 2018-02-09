using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Interfaces
{
    public interface ITokenService
    {
        void SaveToken(string userName, string token);

        bool IsValidToken(string userName, string token);

        void ExtendTokenPeriod(string userName, string token);

        void DeleteTokens(string userName);
    }
}
