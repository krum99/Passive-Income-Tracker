using Microsoft.EntityFrameworkCore;
using PassiveMoneyTracker.Models;

namespace PassiveMoneyTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PassiveIncome> PassiveIncomes { get; set; }

    }
}
