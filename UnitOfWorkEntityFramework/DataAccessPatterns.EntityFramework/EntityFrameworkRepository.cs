using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessPatterns.Contracts;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

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

        #region Get methods

        /// <summary>
        /// Gets a collection including all entities of the repository or a specified page of the output.
        /// </summary>
        /// <param name="pageIndex">Optionally defines the page to get.</param>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entities.</param>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> Get(int pageIndex = 0, int pageSize = int.MaxValue, params Expression<Func<T, object>>[] includePaths)
        {
            return Get(query: null, includePaths: includePaths, pageIndex: pageIndex, pageSize: pageSize);
        }

        /// <summary>
        /// Gets a collection including entities of the repository that meet the specified query criteria or a specified page of the output.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included in the result of the method call.</param>
        /// <param name="pageIndex">Optionally defines the page to get.</param>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entities.</param>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> Get(Func<T, bool> query = null, int pageIndex = 0, int pageSize = int.MaxValue, params Expression<Func<T, object>>[] includePaths)
        {
            return Get<object>(query: query, orderSelector: null, includePaths: includePaths, pageIndex: pageIndex, pageSize: pageSize);
        }

        /// <summary>
        /// Gets a collection including all entities of the repository in the specified order or a specified page of the output.
        /// </summary>
        /// <param name="orderSelector">Defines output collection ordering.</param>
        /// <param name="pageIndex">Optionally defines the page to get.</param>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entities.</param>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> Get<TOrderKey>(Func<T, TOrderKey> orderSelector, int pageIndex = 0, int pageSize = int.MaxValue, params Expression<Func<T, object>>[] includePaths)
        {
            return Get(query: null, orderSelector: orderSelector, pageIndex: pageIndex, pageSize: pageSize, includePaths: includePaths);
        }

        /// <summary>
        /// Gets a collection including entities of the repository that meet the specified query criteria in the specified order or a specified page of the output.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included in the result of the method call.</param>
        /// <param name="orderSelector">Defines output collection ordering.</param>
        /// <param name="pageIndex">Optionally defines the page to get.</param>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entities.</param>
        /// <exception cref="DataAccessException"/>
        public IEnumerable<T> Get<TOrderKey>(Func<T, bool> query, Func<T, TOrderKey> orderSelector, int pageIndex = 0, int pageSize = int.MaxValue, params Expression<Func<T, object>>[] includePaths)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex");
            if (pageSize <= 0 || (pageSize == int.MaxValue && pageIndex > 0))
                throw new ArgumentOutOfRangeException("pageSize");
            var entities = GetEntities(query, orderSelector, includePaths);
            if (pageIndex > 0)
                entities = entities.Skip(pageIndex * pageSize);
            if (pageSize < int.MaxValue)
                entities = entities.Take(pageSize);
            return GetOutputCollection(entities);
        }

        private IEnumerable<T> GetEntities<TOrderKey>(Func<T, bool> query, Func<T, TOrderKey> orderSelector, params Expression<Func<T, object>>[] includePaths)
        {
            var queriableEntities = Entities.AsQueryable();
            if (includePaths != null)
            {
                foreach (var includePath in includePaths)
                    queriableEntities = queriableEntities.Include(includePath);
            }
            var entities = queriableEntities.AsEnumerable();
            if (query != null)
                entities = entities.Where(query);
            if (orderSelector != null)
                entities = entities.OrderBy(orderSelector);
            return entities;
        }

        /// <summary>
        /// Browses the specified enumeration and returns an output read only entity collection.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected IEnumerable<T> GetOutputCollection(IEnumerable<T> entities)
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

        #endregion

        #region PageCount methods

        /// <summary>
        /// Gets the page count for the collection including all entities of the repository or a specified page of the output.
        /// </summary>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <exception cref="DataAccessException"/>
        public int PageCount(int pageSize)
        {
            return PageCount(query: null, pageSize: pageSize);
        }

        /// <summary>
        /// Gets the page count for the collection including entities in the repository that meet the specified query criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be included into the result of the method call.</param>
        /// <param name="pageSize">Optionally defines the page size.</param>
        /// <exception cref="DataAccessException"/>
        public int PageCount(Func<T, bool> query, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize");
            var entities = Entities.AsEnumerable();
            if (query != null)
                entities = entities.Where(query);
            return GetOutputPageCount(entities, pageSize);
        }

        /// <summary>
        /// Browses the specified enumeration and returns the page count for the specified page size.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected int GetOutputPageCount(IEnumerable<T> entities, int pageSize)
        {
            try
            {
                var count = entities.Count();
                if (count == 0)
                    return 0;
                return (count - 1) / pageSize + 1;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region Count methods

        /// <summary>
        /// Gets the count of all entities of the repository.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        public int Count()
        {
            return Count(query: null);
        }

        /// <summary>
        /// Gets the count of entities in the repository that meet the specified query criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entities need to meet to be counted into the result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        public int Count(Func<T, bool> query)
        {
            var entities = Entities.AsEnumerable();
            if (query != null)
                entities = entities.Where(query);
            return GetOutputCount(entities);
        }

        /// <summary>
        /// Browses the specified enumeration and returns the item count.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected int GetOutputCount(IEnumerable<T> entities)
        {
            try
            {
                return entities.Count();
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region Single methods

        /// <summary>
        /// Gets a single entity from the repository meeting the specified selection criteria.
        /// </summary>
        /// <param name="selector">Defines criteria that the single entity needs to meet to be retuned as result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        public T Single(Func<T, bool> selector = null)
        {
            var entities = Entities.AsEnumerable();
            if (selector != null)
                entities = entities.Where(selector);
            return GetSingleOutputItem(entities);
        }

        /// <summary>
        /// Gets a single object from the specified enumeration and returns an output entity.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected T GetSingleOutputItem(IEnumerable<T> entities)
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

        #endregion

        #region SingleOrDefault methods

        /// <summary>
        /// Gets a single entity from the repository meeting the specified selection criteria or the default entity type value (usually null) if none found.
        /// </summary>
        /// <param name="selector">Defines criteria that the single entity needs to meet to be retuned as result of the method call.</param>
        /// <exception cref="DataAccessException"/>
        public T SingleOrDefault(Func<T, bool> selector = null)
        {
            var entities = Entities.AsEnumerable();
            if (selector != null)
                entities = entities.Where(selector);
            return GetSingleOrDefaultOutputItem(entities);
        }

        /// <summary>
        /// Gets a single object from the specified enumeration and returns an output entity or the default entity type value (usually null) if none found.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected T GetSingleOrDefaultOutputItem(IEnumerable<T> entities)
        {
            try
            {
                var item = entities.SingleOrDefault();
                return item;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region First methods

        /// <summary>
        /// Gets the first entity from the repository meeting the specified selection criteria.
        /// </summary>
        /// <param name="query">Defines criteria that the entity needs to meet to be retuned as result of the method call.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entity.</param>
        /// <exception cref="DataAccessException"/>
        public T First(Func<T, bool> query = null, params Expression<Func<T, object>>[] includePaths)
        {
            return First<object>(query: query, orderSelector: null, includePaths: includePaths);
        }

        /// <summary>
        /// Gets the first entity from the repository meeting the specified selection criteria and the specified order.
        /// </summary>
        /// <param name="query">Defines criteria that the entity needs to meet to be retuned as result of the method call.</param>
        /// <param name="orderSelector">Defines collection ordering to consider.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entity.</param>
        /// <exception cref="DataAccessException"/>
        public T First<TOrderKey>(Func<T, bool> query, Func<T, TOrderKey> orderSelector, params Expression<Func<T, object>>[] includePaths)
        {
            var entities = GetEntities(query, orderSelector, includePaths);
            return GetFirstOutputItem(entities);
        }

        /// <summary>
        /// Gets the first object from the specified enumeration and returns an output entity.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected T GetFirstOutputItem(IEnumerable<T> entities)
        {
            try
            {
                var item = entities.First();
                return item;
            }
            catch (Exception exc)
            {
                throw new DataAccessException(exc.Message, exc);
            }
        }

        #endregion

        #region FirstOrDefault methods

        /// <summary>
        /// Gets the first entity from the repository meeting the specified selection criteria or the default entity type value (usually null) if none found.
        /// </summary>
        /// <param name="query">Defines criteria that the entity needs to meet to be retuned as result of the method call.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entity.</param>
        /// <exception cref="DataAccessException"/>
        public T FirstOrDefault(Func<T, bool> query = null, params Expression<Func<T, object>>[] includePaths)
        {
            return FirstOrDefault<object>(query: query, orderSelector: null, includePaths: includePaths);
        }

        /// <summary>
        /// Gets the first entity from the repository meeting the specified selection criteria and the specified order or the default entity type value (usually null) if none found.
        /// </summary>
        /// <param name="query">Defines criteria that the entity needs to meet to be retuned as result of the method call.</param>
        /// <param name="orderSelector">Defines collection ordering to consider.</param>
        /// <param name="includePaths">Optionally defines paths to child entities to load and associate to the returned entity.</param>
        /// <exception cref="DataAccessException"/>
        public T FirstOrDefault<TOrderKey>(Func<T, bool> query, Func<T, TOrderKey> orderSelector, params Expression<Func<T, object>>[] includePaths)
        {
            var entities = GetEntities(query, orderSelector, includePaths);
            return GetFirstOrDefaultOutputItem(entities);
        }

        /// <summary>
        /// Gets the first object from the specified enumeration and returns an output entity or the default entity type value (usually null) if none found.
        /// </summary>
        /// <exception cref="DataAccessException"/>
        protected T GetFirstOrDefaultOutputItem(IEnumerable<T> entities)
        {
            try
            {
                var item = entities.FirstOrDefault();
                return item;
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

        #endregion
    }
}
