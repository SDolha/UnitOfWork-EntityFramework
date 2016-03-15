using System;
using System.Collections.Generic;

namespace DataAccessPatterns.Contracts
{
    // Generic interface for repository pattern.
    public interface IRepository<T>
    {
        // Get methods.
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Func<T, bool> query);
        T GetSingle(Func<T, bool> selector);
        
        // Update methods.
        void Add(T item);
        void Remove(T item);
    }
}
