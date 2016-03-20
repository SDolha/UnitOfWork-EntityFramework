using System;
using System.Collections.Generic;

namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Generic interface for repository pattern.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets a collection including all entities of the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets a collection including entities of the repository that meet the specified query criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included in the result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> Get(Func<T, bool> query);

        /// <summary>
        /// Gets a single entity from the repository meeting the specified selection criteria.
        /// </summary>
        /// <param name="selector">Defines criteria that the single entity needs to meet to be retuned as result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        T GetSingle(Func<T, bool> selector);

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        void Add(T item);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        void Remove(T item);
    }
}
