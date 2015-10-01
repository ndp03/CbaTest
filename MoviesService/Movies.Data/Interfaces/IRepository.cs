using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Interfaces
{
    /// <summary>
    /// A generic interface that contains common CRUD operations for a single entity type.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TEntity, TKey>
    {
        void Delete(TKey id);
        TEntity Get(TKey id);
        IList<TEntity> GetList();
        TKey Insert(TEntity entity);
        void Update(TEntity entity);
    }
}
