using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DbConnection"))
            );

            services.AddMvc(
                options => options.EnableEndpointRouting = false
            );
        }

        /**
         * Configures the HTTP request pipeline.
         * @param app Application builder
         * @param env Hosting environment
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ASPNETCORE_ENVIRONMENT = [ "Development" | "Production" ]
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();

            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            // Allow the web server to use access static file paths in wwwroot folder
            app.UseStaticFiles();

            // Register routes
            app.UseMvc(
                routes => routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}")
            );
        }
    }
}
