using L2C.Budget.BL.Model;
using Microsoft.EntityFrameworkCore;

namespace L2C.Budget.BL.Controller
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=budget40k;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<UserBudget> Budgets { get; set; }
    }
}
