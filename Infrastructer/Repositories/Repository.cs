using Infrastructer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace File.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FileContext context;

        public Repository(FileContext _context)
        {
            this.context = _context;
        }
        public T Add(T entity)
        {
            var newEntity = context.Add(entity);
            return newEntity.Entity;
        }
        public T Update(T entity)
        {

            return context.Update(entity).Entity;
        }
        public T? Get(Guid? id)
        {
            return context.Find<T>(id);
        }
        public IList<T> GetAll()
        {
            return context.Set<T>().ToList();
        }
        public T Delete(T entity)
        {
            return context.Remove(entity).Entity;
        }
        public void SavaChanges()
        {
            context.SaveChanges();
        }
    }
}
