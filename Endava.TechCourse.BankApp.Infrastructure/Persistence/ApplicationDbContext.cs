using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Currency>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Transaction>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Wallet>()
            .Property(e => e.TimeStamp);
        modelBuilder.Entity<Currency>()
            .Property(e => e.TimeStamp);
        modelBuilder.Entity<Transaction>()
            .Property(e => e.TimeStamp);

        modelBuilder.Entity<Currency>()
            .HasMany(e => e.Wallets)
            .WithOne(e => e.Currency)
            .HasForeignKey(e => e.CurrencyId)
            .IsRequired();

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Target)
            .WithMany(x => x.ReceivedTransactions)
            .HasForeignKey(x => x.TargetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Source)
            .WithMany(x => x.InitiatedTransactions)
            .HasForeignKey(x => x.SourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfigurations());
    }
}
