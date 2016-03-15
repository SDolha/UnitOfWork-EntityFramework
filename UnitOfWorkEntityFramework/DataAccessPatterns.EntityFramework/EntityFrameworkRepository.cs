using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessPatterns.Contracts;

namespace DataAccessPatterns.EntityFrameworkImplementation
{
    // Implements IRepository<T> over IDbSet<T>, which is the interface implemented by entity sets generated as properties of database context objects by Entity Framework.
    // Note that the class remains generic, so you can reuse it for any entity set of any database context object.
    public class EntityFrameworkRepository<T> : IRepository<T> where T: class
    {
        public EntityFrameworkRepository(IDbSet<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            this.entities = entities;
        }

        private IDbSet<T> entities;
        public IDbSet<T> Entities
        {
            get { return entities; }
        }

        // Get methods, delegating actions to IDbSet<T>.
        public IEnumerable<T> GetAll()
        {
            return entities;
        }
        public IEnumerable<T> Get(Func<T, bool> query)
        {
            return entities.Where(query);
        }
        public T GetSingle(Func<T, bool> selector)
        {
            return entities.Single(selector);
        }
        
        // Update methods, delegating actions to IDbSet<T>.
        public void Add(T item)
        {
            entities.Add(item);
        }
        public void Remove(T item)
        {
            entities.Remove(item);
        }
    }
}
