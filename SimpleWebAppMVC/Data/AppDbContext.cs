using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Models;

namespace SimpleWebAppMVC.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TaskDbModel> Tasks { get; set; }
    }
}
