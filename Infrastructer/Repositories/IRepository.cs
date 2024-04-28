using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        T? Get(Guid? id);
        IList<T> GetAll();
        T Delete(T entity);
        void SavaChanges();
    }
}
