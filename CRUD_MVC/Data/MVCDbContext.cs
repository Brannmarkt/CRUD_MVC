using CRUD_MVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUD_MVC.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
