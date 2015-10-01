using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Entities;

namespace Movies.Data.Interfaces
{
    public interface IMoviesCacheListRepository : IListRepository<Movie, MovieFilter>
    {
    }
}
