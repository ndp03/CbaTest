using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Interfaces
{
    public interface IListRepository<TEntity, in TFilter> where TEntity : class
    {
        IList<TEntity> GetList(TFilter filter);
    }
}
