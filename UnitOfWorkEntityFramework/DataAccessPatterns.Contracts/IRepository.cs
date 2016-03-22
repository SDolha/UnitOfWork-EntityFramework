using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Generic interface for repository pattern.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets a collection with entities in the repository matching a specified query, optionally loading the related entities specified as include clauses.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Optionally defines related entities to load.</param>
        /// <exception cref="DataAccessException"/>
        IReadOnlyCollection<T> Get(Func<IQueryable<T>, IEnumerable<T>> query = null, params Expression<Func<T, object>>[] includeClauses);

        /// <summary>
        /// Gets a single entity from the repository matching a specified query, optionally loading the related entities specified as include clauses.
        /// </summary>
        /// <param name="selector">Defines the selection query to apply in order to obtain the result.</param>
        /// <param name="includeClauses">Optionally defines related entities to load.</param>
        /// <exception cref="DataAccessException"/>
        T Get(Func<IQueryable<T>, T> selector,  params Expression<Func<T, object>>[] includeClauses);

        /// <summary>
        /// Gets the count of entities in the repository matching a specified query.
        /// </summary>
        /// <param name="query">Defines the query to apply in order to obtain the result.</param>
        /// <exception cref="DataAccessException"/>
        int Count(Func<IQueryable<T>, IEnumerable<T>> query = null);

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        /// <exception cref="DataAccessException"/>
        void Add(T item);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="item">The entity to remove.</param>
        /// <exception cref="DataAccessException"/>
        void Remove(T item);
    }
}
