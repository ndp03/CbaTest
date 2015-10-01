using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Interfaces;
using MoviesLibrary;

namespace Movies.Data
{
    public class Repository 
    {
        protected MovieDataSource Database { get; set; }

        public Repository()
        {
            AutoMapper.Configure();
            Database = new MovieDataSource();
        }
    }
}
