using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Entities;
using MoviesLibrary;

namespace Movies.Data
{
    public class AutoMapper
    {
        public static void Configure()
        {
            Mapper.CreateMap<MovieData, Movie>();

            Mapper.CreateMap<Movie, MovieData>();
        }
    }
}
