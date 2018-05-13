using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SimpleWebAppMVC
{
    public class Program
    {
        /**
         * Builds the web host using the specified arguments and Startup class.
         * @param args Arguments
         */
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}
