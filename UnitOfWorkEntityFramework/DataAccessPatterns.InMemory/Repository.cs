using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccessPatterns.Contracts;
using System.Collections.ObjectModel;

namespace DataAccessPatterns.InMemory
{
    /// <summary>
    /// Implements <see cref="IRepository{T}"/> over a simple, usually in memory, <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">Collection item type</typeparam>
    public class Repository<T> : IRepository<T>
    {
        /// <summary>
        /// Initializes a repository object.
        /// </summary>
        /// <param name="collecion">The item collection to bind the repository to.</param>
        public Repository(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            Collection = collection;
        }

        /// <summary>
        /// The item collection that the repository is bound to.
        /// </summary>
        protected ICollection<T> Collection { get; private set; }

        #region Get methods

        /// <summary>
        /// Gets a collection including items in the repository matching a specified query.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Ignored, since related data for in memory objects should have already been loaded.</param>
        /// <exception cref="DataAccessException"/>
        public IReadOnlyCollection<T> Get(Func<IQueryable<T>, IEnumerable<T>> query = null, params Expression<Func<T, object>>[] includeClauses)
        {
            try
            {
                var queriableItems = Collection.AsQueryable<T>();
                var items = query != null ? query(queriableItems) : queriableItems;
                return new ReadOnlyCollection<T>(items.ToArray());
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Gets a single item from the repository matching a specified query.
        /// </summary>
        /// <param name="selector">Defines the selection query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Ignored, since related data for in memory objects should have already been loaded.</param>
        /// <exception cref="DataAccessException"/>
        public T Get(Func<IQueryable<T>, T> selector, params Expression<Func<T, object>>[] includeClauses)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            try
            {
                var queriableItems = Collection.AsQueryable<T>();
                var item = selector(queriableItems);
                return item;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region Count method

        /// <summary>
        /// Gets the count of items in the repository matching a specified query.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <exception cref="DataAccessException"/>
        public int Count(Func<IQueryable<T>, IEnumerable<T>> query = null)
        {
            try
            {
                var items = query != null ? query(Collection.AsQueryable()) : Collection;
                return items.Count();
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region Update methods

        /// <summary>
        /// Adds an item to the repository.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="DataAccessException"/>
        public void Add(T item)
        {
            try
            {
                Collection.Add(item);
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        /// <summary>
        /// Removes an item from the repository.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="DataAccessException"/>
        public void Remove(T item)
        {
            try
            {
                Collection.Remove(item);
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion
    }
}
