using Infrastructure.Core.Factories;
using Infrastructure.Core.Interfaces.IFactories;
using Infrastructure.Core.Interfaces.IManagers;
using Infrastructure.Core.Interfaces.IServices;
using Infrastructure.Core.Managers;
using Infrastructure.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace TestUI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<ICacheSettingsFactory, CacheSettingsFactory>();
            services.AddSingleton<ISerializationService, SerializationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            //when debugging locally working directory is your project's directory with nlog.config inside, 
            // but after publishing to IIS all source files are in %PATH_TO_PUBLISHED_SERVICE%\approot\src\%PROJECT_NAME%\ while working directory is %PATH_TO_PUBLISHED_SERVICE%\

            // env.ConfigureNLog(Path.Combine(env.ContentRootPath, "nlog.config"));

            app.UseDeveloperExceptionPage();
           // app.UseBrowserLink();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
