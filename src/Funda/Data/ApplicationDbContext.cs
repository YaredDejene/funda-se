using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Funda.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<House> Houses { get; set; }
    }
}