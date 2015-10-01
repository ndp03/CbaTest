using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Movies.Data.Interfaces;
using Movies.Entities;
using MoviesLibrary;

namespace Movies.Data.Movies
{
    public class MoviesRepository : Repository, IMoviesRepository
    {
        [Obsolete("Not supported")]
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Movie Get(int id)
        {
            Movie result = null;
            var movie = Database.GetDataById(id);

            if (movie != null)
            {
                result = Mapper.Map<MovieData, Movie>(movie);
            }

            return result;
        }

        public IList<Movie> GetList()
        {
            var result = Mapper.Map<List<MovieData>, List<Movie>>(Database.GetAllData());

            return result;
        }

        public int Insert(Movie movie)
        {
            var movieData = Mapper.Map<Movie, MovieData>(movie);

            return Database.Create(movieData);
        }

        public void Update(Movie movie)
        {
            var movieData = Mapper.Map<Movie, MovieData>(movie);

            Database.Update(movieData);
        }
    }
}
