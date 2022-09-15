using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartIK.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        internal object ApplicationUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }
        public DbSet<Corporation> Corporations { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<AdvancePayment> AdvancePayment { get; set; }
        public DbSet<Permission> Permissions{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Wallet>().HasOne(x => x.Corporation).WithOne(x => x.Wallet).OnDelete(DeleteBehavior.SetNull);
        }
    }
}