using Bonus.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Products.API.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Incentive> Incentives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Incentive>().HasData(new Incentive
            {
                IncentiveId = 1,
                IncentiveCode = "Inc1",
                MinTransportCount = 2,
                MinKilometersCount = 300,
                Bonus = 2000,
            });
            modelBuilder.Entity<Incentive>().HasData(new Incentive
            {
                IncentiveId = 2,
                IncentiveCode = "Inc2",
                MinTransportCount = 3,
                MinKilometersCount = 400,
                Bonus = 3000,
            });
        }
    }
}