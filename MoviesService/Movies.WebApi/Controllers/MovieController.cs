using System.Collections.Generic;
using System.Web.Http;
using Movies.Core.Interfaces;
using Movies.Entities;
using Movies.Utility.Sorting;

namespace MoviesWebApi.Controllers
{
    public class MoviesController : ApiController
    {
        private readonly IMoviesService _moviesService;

        public MoviesController (IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PagedList<Movie> GetAll()
        {
            return _moviesService.GetPagedList();
        }

        /// <summary>
        /// 
        /// Search movie by field or all fields
        /// </summary>
        /// <param name="searchBy">Leave this empty to search across all fields</param>
        /// <param name="searchTerm">The term that we want to search for</param>
        /// <returns></returns>
        [Route("api/movies/search")]
        public PagedList<Movie> Search(string searchTerm, string searchBy = null)
        {
            return _moviesService.GetPagedList(new MovieFilter
            {
                SearchBy = searchBy,
                SearchText = searchTerm
            });
        }

        /// <summary>
        /// Get a movie by MovieId
        /// </summary>
        /// <param name="id">the MovieId</param>
        /// <returns></returns>
        [HttpGet]
        public Movie Get(int id)
        {
            return _moviesService.Get(id);
        }

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <param name="movie">the new movie to be added</param>
        /// <returns>The new MovieId</returns>
        [Route("api/movies/create")]
        public int Create(Movie movie)
        {
            return _moviesService.Create(movie);
        }

        /// <summary>
        /// Updated an existing movie
        /// </summary>
        /// <param name="movie">the movie object to be modified</param>
        [Route("api/movies/update")]
        public void Update(Movie movie)
        {
            _moviesService.Update(movie);
        }
    }
}
