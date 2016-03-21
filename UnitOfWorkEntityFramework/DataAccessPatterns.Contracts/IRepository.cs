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
        /// Gets a collection including all entities of the repository or a specified page of the output.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> Get(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a collection including entities in the repository that meet the specified query criteria or a specified page of the output.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included into the result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> Get(Func<T, bool> query, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a collection including all entities of the repository in the specified order or a specified page of the output.
        /// </summary>
        /// <param name="orderSelector">Defines output collection ordering.</param>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> Get<TOrderKey>(Func<T, TOrderKey> orderSelector, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a collection including entities in the repository that meet the specified query criteria in the specified order or a specified page of the output.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included into the result of the method call.</param>
        /// <param name="orderSelector">Defines output collection ordering.</param>
        /// <exception cref="DataAccessException"/>
        IEnumerable<T> Get<TOrderKey>(Func<T, bool> query, Func<T, TOrderKey> orderSelector, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets the count of all entities of the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        int CountAll();

        /// <summary>
        /// Gets the count of entities in the repository that meet the specified query criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be counted into the result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        int Count(Func<T, bool> query);

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
