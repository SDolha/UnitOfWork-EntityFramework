using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataAccessPatterns.Contracts;

namespace DataAccessPatterns.EntityFramework
{
    /// <summary>
    /// Implements <see cref="IRepository{T}"/> over <see cref="IDbSet{T}"/> which is the interface implemented by entity sets generated as properties of database context objects by Entity Framework.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T: class
    {
        /// <summary>
        /// Initializes a repository object.
        /// </summary>
        /// <param name="entities">The entity set to bind the repository to.</param>
        public Repository(IDbSet<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            Entities = entities;
        }

        /// <summary>
        /// The entity set that the repository is bound to.
        /// </summary>
        protected IDbSet<T> Entities { get; private set; }

        #region Get methods

        /// <summary>
        /// Gets a collection including entities in the repository matching a specified query, optionally loading the related entities specified as include clauses.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Optionally defines related entities to load.</param>
        /// <exception cref="DataAccessException"/>
        public IReadOnlyCollection<T> Get(Func<IQueryable<T>, IEnumerable<T>> query = null, params Expression<Func<T, object>>[] includeClauses)
        {
            try
            {
                var queriableEntities = GetQueriableEntitiesIncluding(includeClauses);
                var entities = query != null ? query(queriableEntities) : queriableEntities;
                return new ReadOnlyCollection<T>(entities.ToArray());
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Gets a single entity from the repository matching a specified query, optionally loading the related entities specified as include clauses.
        /// </summary>
        /// <param name="selector">Defines the selection query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Optionally defines related entities to load.</param>
        /// <exception cref="DataAccessException"/>
        public T Get(Func<IQueryable<T>, T> selector, params Expression<Func<T, object>>[] includeClauses)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            try
            {
                var queriableEntities = GetQueriableEntitiesIncluding(includeClauses);
                var entity = selector(queriableEntities);
                return entity;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        private IQueryable<T> GetQueriableEntitiesIncluding(Expression<Func<T, object>>[] includeClauses)
        {
            var entities = Entities.AsQueryable();
            foreach (var includeClause in includeClauses)
                entities = entities.Include(includeClause);
            return entities;
        }

        #endregion

        #region Count method

        /// <summary>
        /// Gets the count of entities in the repository matching a specified query.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <exception cref="DataAccessException"/>
        public int Count(Func<IQueryable<T>, IEnumerable<T>> query = null)
        {
            try
            {
                var entities = query != null ? query(Entities) : Entities;
                return entities.Count();
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region Update methods

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
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
        /// <param name="item">The entity to remove.</param>
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

        #endregion
    }
}
