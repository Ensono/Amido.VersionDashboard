// ©2015 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 Licence (http://www.apache.org/licenses/LICENSE-2.0)
// ReSharper disable RedundantUsingDirective

using System;
using System.Web;
using System.Web.Http;
using Amido.VersionDashboard.Data;
using Amido.VersionDashboard.Data.DocumentDb;
using Amido.VersionDashboard.Data.FileSystem;
using Amido.VersionDashboard.Network;
using Amido.VersionDashboard.Network.FileSystem;
using Amido.VersionDashboard.Network.Json;
using Amido.VersionDashboard.Web.App_Start;
using Amido.VersionDashboard.Web.Domain;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace Amido.VersionDashboard.Web.App_Start {
    public static class NinjectWebCommon {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start() {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop() {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel() {
            var kernel = new StandardKernel();
            try {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            } catch {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel) {
            kernel.Bind<INavigationProvider>().To<NavigationProvider>();
            kernel.Bind<IConfigProvider>().To<AppSettingsConfigProvider>();

#if DEBUG
            var dataFolder = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            kernel.Bind<IRequestProxy>().To<FileSystemRequestProxy>().WithConstructorArgument(dataFolder);
            kernel.Bind<IDataStore>().To<FileSystemDataStore>().InSingletonScope().WithConstructorArgument(dataFolder);
#else
            kernel.Bind<IRequestProxy>().To<JsonRequestProxy>();
            kernel.Bind<IDataStore>().To<DocumentDbDataStore>().InSingletonScope();
#endif

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}