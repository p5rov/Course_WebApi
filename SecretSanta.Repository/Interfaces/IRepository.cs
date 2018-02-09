using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        IDbSet<T> Set();

        T FindById(object id);

        int SaveChanges();
    }
}
