using Endava.TechCourse.BankApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>().HasKey(x => x.Id);
        modelBuilder.Entity<Currency>().HasKey(x => x.Id);

        modelBuilder.Entity<Wallet>()
            .Property(e => e.TimeStamp);
        modelBuilder.Entity<Currency>()
            .Property(e => e.TimeStamp);

        base.OnModelCreating(modelBuilder);
    }
}
