using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleWebAppMVC
{
    /**
     * Configures services and the app request pipeline.
     */
    public class Startup
    {
        public IConfiguration Configuration { get; }

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
            services.AddMvc();
        }

        /**
         * Configures the HTTP request pipeline.
         * @param app Application builder
         * @param env Hosting environment
         */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            // Allow the web server to use access static file paths in wwwroot folder
            app.UseStaticFiles();

            // Register routes
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
