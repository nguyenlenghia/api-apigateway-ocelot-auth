using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

namespace ApiGatewayOcelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //return WebHost.CreateDefaultBuilder(args)
            //    .UseUrls("http://localhost:8099")
            //    .ConfigureAppConfiguration((hostingContext, config) =>
            //    {
            //        config
            //            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            //            .AddJsonFile("appsettings.json", true, true)
            //            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            //            .AddJsonFile("ocelot.json", false, false)
            //            .AddEnvironmentVariables();
            //    })
            //    .ConfigureServices(s =>
            //    {
            //        s.AddOcelot().AddEureka().AddCacheManager(x => x.WithDictionaryHandle());
            //    })
            //    .Configure(a =>
            //    {
            //        a.UseOcelot().Wait();
            //    });

            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    //.AddJsonFile("appsettings.json", true, true)
                    //.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                    //.AddOcelot(hostingContext.HostingEnvironment)
                    .AddEnvironmentVariables();
            });
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseStartup<Startup>()
               .UseUrls("http://localhost:9000");
            return builder;
        }

        //WebHost.CreateDefaultBuilder(args)
        //.ConfigureAppConfiguration((hostingContext, config) =>
        //{
        //    config
        //        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        //        .AddJsonFile("ocelot.json", false, false)
        //        .AddEnvironmentVariables();
        //})
        //.UseStartup<Startup>();

        //WebHost.CreateDefaultBuilder(args)
        //.ConfigureAppConfiguration((hostingContext, config) =>
        //{
        //config
        //.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        //.AddJsonFile("appsettings.json", true, true)
        //.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        //.AddJsonFile("ocelot.json", false, false)
        //.AddEnvironmentVariables();
        //})
        //.ConfigureServices(s =>
        //{
        ////s.AddOcelot().AddEureka().AddCacheManager(x => x.WithDictionaryHandle());
        //})
        //.Configure(a =>
        //{
        //a.UseOcelot().Wait();
        //})
        //.Build();


    }
}
