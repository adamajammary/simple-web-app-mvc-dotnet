using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using SimpleWebAppMVC.Data;

namespace SimpleWebAppMVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // DB context
            bool   useMySQL         = this.Configuration.GetValue<bool>("UseMySQL");
            string connectionString = this.Configuration.GetConnectionString("DbConnection");

            if (useMySQL)
                services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            else
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // MVC
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // Swagger UI
            services.AddSwaggerDocument(settings => {
                settings.PostProcess = document => {
                    document.Info.Title       = "Simple Web API";
                    document.Info.Description = "A simple ASP.NET web API";
                    document.Info.Version     = "v1";
                };
            });
        }

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

            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-7.0
            // https://tutexchange.com/how-to-host-asp-net-core-app-on-ubuntu-with-apache-webserver/
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Allow the web server to access static file paths in wwwroot folder
            app.UseStaticFiles();

            // Swagger UI
            app.UseOpenApi(settings => {
                settings.PostProcess = (document, _) => {
                    document.Schemes = new[] {
                        env.IsDevelopment() ? OpenApiSchema.Http : OpenApiSchema.Https
                    };
                };
            });

            app.UseSwaggerUi3();

            // Register routes
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
