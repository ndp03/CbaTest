using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movies.Core.Interfaces;
using Movies.Core.Movies;
using Movies.Data.Interfaces;
using Movies.Data.Movies;
using Movies.Entities;
using Movies.Utility.Interfaces;
using Movies.Utility.Logging;
using Movies.Utility.Sorting;
using MoviesLibrary;

namespace MoviesWebApiTest
{
    /// <summary>
    /// A set of tests with Mock data to test our sorting. 
    /// This is suitable for automated unit test.
    /// </summary>
    [TestClass]
    public class MoviesServiceWithMockTest
    {
        private IMoviesService _moviesService;

        [TestInitialize]
        public void SetUp()
        {
            var cache = new Mock<ICacheProvider>();
            _moviesService = new MoviesService(new LogProvider(), cache.Object, GetMockMovieRepository());
        }

        [TestMethod]
        public void GetMovieIdListUnsorted()
        {
            var movies = _moviesService.GetPagedList();
            var expectMovieIdList = new List<int> { 3, 1, 4, 2 };
            var sortedMovieIdList = movies.Items.Select(x => x.MovieId).ToList();
            CollectionAssert.AreEqual(expectMovieIdList, sortedMovieIdList);
        }

        [TestMethod]
        public void SortMovieIdAscending()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SortBy = "MovieId",
                SortOrder = SortDirection.Ascending
            });

            var expectMovieIdList = new List<int> { 1, 2, 3, 4 };
            var sortedMovieIdList = movies.Items.Select(x => x.MovieId).ToList();
            CollectionAssert.AreEqual(expectMovieIdList, sortedMovieIdList);
        }

        [TestMethod]
        public void SortMovieIdDescending()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SortBy = "MovieId",
                SortOrder = SortDirection.Descending
            });

            var expectMovieIdList = new List<int> {4, 3, 2, 1};
            var sortedMovieIdList = movies.Items.Select(x => x.MovieId).ToList();
            CollectionAssert.AreEqual(expectMovieIdList, sortedMovieIdList);
        }

        [TestMethod]
        public void SortByTitleAscending()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SortBy = "Title",
                SortOrder = SortDirection.Ascending
            });

            var expectedList = new List<string> { "A movie", "B movie", "C movie", "D movie" };
            var sortedList = movies.Items.Select(x => x.Title).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);
        }

        [TestMethod]
        public void SortByTitleDescending()
        {
            var movies = _moviesService.GetPagedList(new MovieFilter
            {
                SortBy = "Title",
                SortOrder = SortDirection.Descending
            });

            var expectedList = new List<string> { "D movie", "C movie", "B movie", "A movie" };
            var sortedList = movies.Items.Select(x => x.Title).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);
        }

        private IMoviesRepository GetMockMovieRepository()
        {
            var mockRepository = new Mock<IMoviesRepository>();

            mockRepository.Setup(x => x.GetList()).Returns(new List<Movie>
            {
                new Movie
                {
                    MovieId = 3,
                    Title = "C movie",
                    Classification = "PG",
                    Genre = "Action",
                    Rating = 3,
                    ReleaseDate = 1996,
                    Cast = new[] {"Actor 3", "Actor 12"}
                },
                new Movie
                {
                    MovieId = 1,
                    Title = "A movie",
                    Classification = "G",
                    Genre = "Comedy",
                    Rating = 3,
                    ReleaseDate = 1996,
                    Cast = new[] {"Actor 1", "Actor 2"}
                },
                new Movie
                {
                    MovieId = 4,
                    Title = "D movie",
                    Classification = "R",
                    Genre = "Drama",
                    Rating = 3,
                    ReleaseDate = 1996,
                    Cast = new[] {"Actor X", "Actor Y"}
                }
                ,
                new Movie
                {
                    MovieId = 2,
                    Title = "B movie",
                    Classification = "R",
                    Genre = "Thriller",
                    Rating = 3,
                    ReleaseDate = 1996,
                    Cast = new[] {"Actor 4", "Actor 7"}
                }
              
            });

            return mockRepository.Object;
        }
    }
}
