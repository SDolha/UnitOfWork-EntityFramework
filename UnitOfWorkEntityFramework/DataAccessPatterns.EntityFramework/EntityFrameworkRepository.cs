using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessPatterns.Contracts;
using System.Collections.ObjectModel;

namespace DataAccessPatterns.EntityFramework
{
    /// <summary>
    /// Implements <see cref="IRepository{T}"/> over <see cref="IDbSet{T}"/> which is the interface implemented by entity sets generated as properties of database context objects by Entity Framework.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class EntityFrameworkRepository<T> : IRepository<T> where T: class
    {
        public EntityFrameworkRepository(IDbSet<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            Entities = entities;
        }

        protected IDbSet<T> Entities { get; private set; }

        /// <summary>
        /// Gets a collection including all entities of the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> GetAll()
        {
            return GetItemCollection(Entities);
        }

        /// <summary>
        /// Gets a collection including entities of the repository that meet the specified query criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included in the result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> Get(Func<T, bool> query)
        {
            return GetItemCollection(Entities.Where(query));
        }

        /// <summary>
        /// Browses the specified enumeration and returns an output read only entity collection.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected IEnumerable<T> GetItemCollection(IEnumerable<T> entities)
        {
            try
            {
                var list = entities.ToList();
                return new ReadOnlyCollection<T>(list);
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Gets a single entity from the repository meeting the specified selection criteria.
        /// </summary>
        /// <param name="selector">Defines criteria that the single entity needs to meet to be retuned as result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        public T GetSingle(Func<T, bool> selector)
        {
            return GetItem(Entities.Where(selector));
        }

        /// <summary>
        /// Gets a single object from the specified enumeration and returns an output entity.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected T GetItem(IEnumerable<T> entities)
        {
            try
            {
                var item = entities.Single();
                return item;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        public void Add(T item)
        {
            try
            {
                Entities.Add(item);
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        public void Remove(T item)
        {
            try
            {
                Entities.Remove(item);
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }
    }
}
