using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Models;

namespace SimpleWebAppMVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskDbModel> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}
