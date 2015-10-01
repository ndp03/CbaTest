using System;
using System.Collections.Generic;
using System.Linq;
using HVManager.Util.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Interfaces;
using Movies.Data.Interfaces;
using Movies.Entities;
using Movies.Utility.ExtensionMethods;
using Movies.Utility.Interfaces;
using Movies.Utility.Sorting;

namespace Movies.Core.Movies
{
    public class MoviesService : IMoviesService
    {
        private const string BaseMovieDataCacheKey = "Movies.Core.Movies.MoviesService";

        private const int MovieDataCacheExpirationInHours = 24;

        private const string NotSortableAttribute = "Cast";

        private readonly ICacheProvider _cacheProvider;

        private readonly IMoviesRepository _moviesRepository;

        private readonly ILogProvider _logProvider;

        public MoviesService(
            ILogProvider logProvider,
            ICacheProvider cacheProvider, 
            IMoviesRepository moviesRepository)
        {
            _cacheProvider = cacheProvider;
            _logProvider = logProvider;
            _moviesRepository = moviesRepository;
        }

        public PagedList<Movie> GetPagedList(MovieFilter filter = null)
        {
            PagedList<Movie> result;

            if (filter == null)
            {
                filter = new MovieFilter();
            }

            _logProvider.LogDebug("Enter MoviesService.GetPagedList() with filter : " + filter);

            if (filter.SortBy.IsNotNullOrEmpty() && filter.SortBy.Equals(NotSortableAttribute))
            {
                throw new ArgumentException(NotSortableAttribute + " is not a sortable attribute");
            }

            try
            {
                // Get the cache key based on filter parameters
                var cacheKey = GetCacheKey(filter);

                var filteredMovies = _cacheProvider.GetCacheValue<List<Movie>>(cacheKey);

                if (filteredMovies == null)
                {
                    var allMovies = GetAllMovies();
                    var searchText = filter.SearchText;
                    
                    if (searchText.IsNotNullOrEmpty() && filter.SearchBy.IsNotNullOrEmpty())
                    {
                        // Search by a property
                        filteredMovies = SearchByProperty(
                            allMovies, 
                            filter.SearchBy, 
                            searchText, 
                            filter.IsCaseSensitive).ToList();
                    }
                    else
                    {
                        // Search across all properties
                        filteredMovies = allMovies
                            .Where(m => searchText.IsNullOrEmpty() // if SearchText is empty, no filter apply
                                        || m.MovieId.ToString() == searchText
                                        || m.Title.Contains(searchText, filter.IsCaseSensitive)
                                        || m.Classification.Contains(searchText, filter.IsCaseSensitive)
                                        || m.Genre.Contains(searchText)
                                        || filter.SearchText == m.Rating.ToString()
                                        || filter.SearchText == m.ReleaseDate.ToString())
                            .ToList();
                    }

                    _cacheProvider.SetCacheValue(
                        cacheKey, 
                        filteredMovies, 
                        null, 
                        new TimeSpan(0, MovieDataCacheExpirationInHours, 0, 0));
                }

                result = Pager.Paginate(filteredMovies, new PagerSettings
                {
                    PageNumber = filter.CurrentPageIndex,
                    PageSize = filter.PageSize,
                    SortBy = filter.SortBy,
                    SortDirection = filter.SortOrder,
                    Paginate = true
                });

            }
            catch (Exception e)
            {
                _logProvider.LogException(e);

                throw new MoviesServiceException(e.Message);
            }

            return result;
        }

        public Movie Get(int movieId)
        {
            _logProvider.LogDebug("Enter MoviesService.Get() with movieId: " + movieId);
            return _moviesRepository.Get(movieId);
        }

        public int Create(Movie movie)
        {
            _logProvider.LogDebug("Enter MoviesService.Create()" );

            try
            {
                var movieId = _moviesRepository.Insert(movie);
                ClearCache();

                return movieId;
            }
            catch (Exception ex)
            {
                _logProvider.LogException(ex);
                throw new MoviesServiceException(ex.Message);
            }
        }

        public void Update(Movie movie)
        {
            _logProvider.LogDebug("Enter MoviesService.Update()");
            
            try
            {
               _moviesRepository.Update(movie);
                ClearCache();
            }
            catch (Exception ex)
            {
                _logProvider.LogException(ex);
                throw new MoviesServiceException(ex.Message);
            }
        }

        /// <summary>
        /// Get and cache all movies
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Movie> GetAllMovies()
        {
            var allMovies = _cacheProvider.GetCacheValue<List<Movie>>(BaseMovieDataCacheKey);

            if (allMovies == null || !allMovies.Any())
            {
                allMovies = _moviesRepository.GetList().ToList();
                _cacheProvider.SetCacheValue<List<Movie>>(BaseMovieDataCacheKey, allMovies, null, new TimeSpan(0, MovieDataCacheExpirationInHours, 0, 0));
            }

            return allMovies;
        }

        // Construct a cache key based on filter parameters
        private string GetCacheKey(MovieFilter filter)
        {
            return string.Format("{0}.SearchBy={1}.SearchText={2}.IsCaseSensitive={3}", BaseMovieDataCacheKey, filter.SearchBy, filter.SearchText, filter.IsCaseSensitive);
        }

        private IEnumerable<Movie> SearchByProperty(
            IEnumerable<Movie> movies, 
            string propertyName, 
            string searchTerm, 
            bool isCaseSensitive)
        {
            if (typeof(Movie).GetProperty(propertyName) == null)
            {
                throw new MoviesServiceException(propertyName + " property does not exist.");
            }

            Func<Movie, bool> expression = item =>
            {
                var propertyValue = item.GetType().GetProperty(propertyName).GetValue(item, null);

                if (propertyValue is int)
                {
                    return propertyValue.ToString() == searchTerm;
                }
                else if (propertyValue is string)
                {
                    return ((string) propertyValue).Contains(searchTerm, isCaseSensitive);
                }
                else
                {
                    // TODO: define other matching rules if need to
                    return Equals(propertyValue, searchTerm);
                }
            };

            return movies.Where(expression);
        }

        // Removed all cache objects that is used by this service
        public void ClearCache()
        {
            _cacheProvider.RemoveCacheContains(BaseMovieDataCacheKey);
        }
    }
}
