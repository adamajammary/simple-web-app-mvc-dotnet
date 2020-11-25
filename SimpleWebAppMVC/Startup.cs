using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleWebAppMVC.Data;

namespace SimpleWebAppMVC
{
    /**
     * Configures services and the app request pipeline.
     */
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        /**
         * Startup constructor.
         * @param configuration Configuration
         */
        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        /**
         * Adds services to the container.
         * @param services Service collection
         */
        public void ConfigureServices(IServiceCollection services)
        {
            bool   useMySQL         = this.Configuration.GetValue<bool>("UseMySQL");
            string connectionString = this.Configuration.GetConnectionString("DbConnection");

            if (useMySQL)
                services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString));
            else
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        /**
         * Configures the HTTP request pipeline.
         * @param app Application builder
         * @param env Hosting environment
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ASPNETCORE_ENVIRONMENT = [ "Development" | "Production" ]
            string jsonFile = (env.IsProduction() ? "appsettings.json" : $"appsettings.{env.EnvironmentName}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(jsonFile, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();

            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-3.1
            // https://tutexchange.com/how-to-host-asp-net-core-app-on-ubuntu-with-apache-webserver/
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Allow the web server to access static file paths in wwwroot folder
            app.UseStaticFiles();

            // Register routes
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
