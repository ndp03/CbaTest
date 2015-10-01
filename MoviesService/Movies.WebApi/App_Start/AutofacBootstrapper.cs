using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Movies.Core.Interfaces;
using Movies.Core.Movies;
using Movies.Data.Interfaces;
using Movies.Data.Movies;
using Movies.Utility.Caching;
using Movies.Utility.Interfaces;
using Movies.Utility.Logging;

namespace MoviesWebApi.App_Start
{
    public class AutofacBootstrapper
    {
        public static void Initialize()
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register other dependencies.
            builder.RegisterType<HttpCacheProvider>().As<ICacheProvider>();
            builder.RegisterType<MoviesService>().As<IMoviesService>();
            builder.RegisterType<MoviesRepository>().As<IMoviesRepository>();
            builder.RegisterType<LogProvider>().As<ILogProvider>();

            // Build the container.
            var container = builder.Build();

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}