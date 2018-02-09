using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Repository.Interfaces
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext m_Context;
        
        public Repository(DbContext context)
        {
            m_Context = context;
        }

        public IDbSet<T> Set()
        {
            return m_Context.Set<T>();
        }

        public T FindById(object id)
        {
            return m_Context.Set<T>().Find(id);
        }
        
        public int SaveChanges()
        {
            return m_Context.SaveChanges();
        }
    }
}
