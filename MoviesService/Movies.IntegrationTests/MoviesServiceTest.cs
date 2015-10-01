using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Interfaces;
using Movies.Core.Movies;
using Movies.Data.Movies;
using Movies.Entities;
using Movies.Utility.Caching;
using Movies.Utility.Interfaces;
using Movies.Utility.Logging;
using Movies.Utility.Sorting;
using MoviesLibrary;
using Moq;

namespace MoviesWebApiTest
{
    /// <summary>
    /// These are a set of integration tests
    /// to check if our service does work.
    /// This is not suitable for automated unit test.
    /// </summary>
    [TestClass]
    public class MoviesServiceIntegrationTest
    {
        private IMoviesService _moviesService;

        [TestInitialize]
        public void SetUp()
        {
            var cache = new Mock<ICacheProvider>();
            _moviesService = new MoviesService(new LogProvider(), cache.Object, new MoviesRepository());
        }

        [TestMethod]
        public void GetAllData()
        {
            var movieDataSource = new MovieDataSource();
            var allMovies = movieDataSource.GetAllData();
            Assert.IsTrue(allMovies.Any());
        }

        [TestMethod]
        public void GetAllMovies()
        {
            var allMovies = _moviesService.GetPagedList();

            Assert.IsTrue(allMovies.TotalItems > 0);
        }

        [TestMethod]
        public void GetOneMovie()
        {
            var movie = _moviesService.Get(1);

            Assert.IsTrue(movie != null);
            Assert.IsTrue(movie.MovieId == 1);
        }

        [TestMethod]
        public void AddOneMovie_ThrowsException()
        {
            var movie = new Movie
            {
            };

            var exception = ExceptionAssert.Throws<MoviesServiceException>(
                () => _moviesService.Create(movie));

            Assert.IsTrue(exception.Message.Contains("Movie Title is mandatory"));
        }

        [TestMethod]
        public void AddOneMovie()
        {
            var movie = new Movie
            {
                Title = "Movie title",
                Rating = 1,
                ReleaseDate = 2000
            };

            var movieId = _moviesService.Create(movie);
            var movieJustAdd = _moviesService.Get(movieId);

            Assert.IsTrue(movieJustAdd != null);
        }

        [TestMethod]
        public void UpdateMovie_ThrowsException()
        {
            var movie = new Movie
            {
                MovieId = 99999999,
                Title = "Updated Movie title",
            };

            var exception = ExceptionAssert.Throws<MoviesServiceException>(
                () => _moviesService.Update(movie));

            Assert.IsTrue(exception.Message.Contains("The movie ID 99999999 does not exist"));
        }

        [TestMethod]
        public void SearchByGenre()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SearchText = "Thriller",
                SearchBy = "Genre"
            });


            Assert.IsTrue(movies.TotalItems > 0);
        }

        [TestMethod]
        public void SearchAcrossAllProperties()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SearchText = "ill"
            });

            Assert.IsTrue(movies.Items.Any(x => x.Title == "We're The Millers"));
            Assert.IsTrue(movies.Items.Any(x => x.Genre == "Thriller"));
        }

        [TestMethod]
        public void SearchByNotExistProperty_ThrowsException()
        {
            var fakeProp = "FakeProperty";

            var exception = ExceptionAssert.Throws<MoviesServiceException>(
                () => _moviesService.GetPagedList(new MovieFilter
                    {
                        SearchText = "something",
                        SearchBy = fakeProp
                    }));

            Assert.IsTrue(exception.Message.Contains(" does not exist"));
        }

        // search case sensitive
        // search case in-sensitive

    }
}
