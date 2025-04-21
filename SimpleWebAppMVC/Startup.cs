using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using SimpleWebAppMVC.Data;
using System.Text;

namespace SimpleWebAppMVC
{
    public class Startup(IConfiguration cfg)
    {
        private IConfiguration config = cfg;

        public void ConfigureServices(IServiceCollection services)
        {
            // DB context

            bool   useMySQL         = this.config.GetValue<bool>("UseMySQL");
            string connectionString = this.config.GetConnectionString("DbConnection");

            if (useMySQL)
                services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            else
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // MVC

            services.AddMvc(options => options.EnableEndpointRouting = false);

            // Swagger

            services.AddOpenApiDocument(settings =>
            {
                settings.AddSecurity("Bearer", new OpenApiSecurityScheme {
                    Name         = "Authorization",
                    Type         = OpenApiSecuritySchemeType.ApiKey,
                    Scheme       = "Bearer",
                    BearerFormat = "JWT",
                    In           = OpenApiSecurityApiKeyLocation.Header,
                    Description  = "Authorization header, example: 'Bearer ey...'.\n\nCall '/api/account/login' to get the token.",
                    
                });

                settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

                settings.PostProcess = document => {
                    document.Info.Title       = "Simple REST API";
                    document.Info.Description = "A simple ASP.NET Web API";
                    document.Info.Version     = "v1";
                };
            });

            // Auth

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication().AddCookie().AddJwtBearer(options =>
            {
                var tokenSettings = this.config.GetRequiredSection("Token");

                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience         = true,
                    ValidateIssuer           = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime         = true,
                    ValidAudience    = tokenSettings.GetValue<string>("Audience"),
                    ValidIssuer      = tokenSettings.GetValue<string>("Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.GetValue<string>("Key"))),
                };
            });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ASPNETCORE_ENVIRONMENT = [ "Production" | "Development" ]

            string jsonFile = (env.IsProduction() ? "appsettings.json" : $"appsettings.{env.EnvironmentName}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(jsonFile, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.config = builder.Build();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-9.0
            // https://tutexchange.com/how-to-host-asp-net-core-app-on-ubuntu-with-apache-webserver/

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Allow the web server to access static file paths in wwwroot folder

            app.UseStaticFiles();

            // Auth

            app.UseAuthentication();
            app.UseAuthorization();

            // Swagger

            app.UseOpenApi(settings => {
                settings.PostProcess = (document, _) => {
                    document.Schemes = [ env.IsDevelopment() ? OpenApiSchema.Http : OpenApiSchema.Https ];
                };
            });

            app.UseSwaggerUi(settings => settings.DocExpansion = "list");

            // MVC

            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
