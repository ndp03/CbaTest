using Movies.Entities;
using Movies.Utility.Sorting;

namespace Movies.Core.Interfaces
{
    public interface IMoviesService
    {
        /// <summary>
        /// Get one movie by primary key
        /// </summary>
        /// <param name="movieId">movieId</param>
        /// <returns></returns>
        Movie Get(int movieId);

        /// <summary>
        /// Add a new movie
        /// </summary>
        /// <param name="movie">the target movie</param>
        /// <returns></returns>
        int Create(Movie movie);

        /// <summary>
        /// Update an existing movie by its Id
        /// </summary>
        /// <param name="movie">the target movie</param>
        void Update(Movie movie);

        /// <summary>
        /// Get a paginated list of all movies with filter option
        /// </summary>
        /// <param name="filter">Contains search and sort parameters. For searching, we will try to 
        /// do an equality comparison for integer and a contain comparison for string. 
        /// Supports case-sensitive or case-insensitive</param>
        /// <returns></returns>
        PagedList<Movie> GetPagedList(MovieFilter filter = null);
    }
}
