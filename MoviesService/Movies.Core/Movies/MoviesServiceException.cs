using System;

namespace Movies.Core.Movies
{
    public class MoviesServiceException : Exception
    {
        public MoviesServiceException(string message) : base(message) { }

    }
}
