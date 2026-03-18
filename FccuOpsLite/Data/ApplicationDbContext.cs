using FccuOpsLite.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FccuOpsLite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members => Set<Member>();
        public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Member>()
                .HasIndex(m => m.MemberNumber)
                .IsUnique();

            builder.Entity<LoanApplication>()
                .Property(l => l.Status)
                .HasConversion<string>();
        }
    }
}