using System;

namespace Movies.Utility.Interfaces
{
    public interface ILogProvider
    {
        void LogException(Exception e);

        void LogDebug(string message);
    }
}