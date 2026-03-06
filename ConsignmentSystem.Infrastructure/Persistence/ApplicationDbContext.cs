using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Domain.Common;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ConsignmentSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid,
        IdentityUserClaim<Guid>,
        ApplicationUserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<ConsignmentReceipt> ConsignmentReceipts => Set<ConsignmentReceipt>();
        public DbSet<ConsignmentItem> ConsignmentItems => Set<ConsignmentItem>();
        public DbSet<SaleTransaction> SaleTransactions => Set<SaleTransaction>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
            builder.Entity<ApplicationRole>(b => b.ToTable("Roles"));
            builder.Entity<ApplicationUserRole>(b => b.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens"));

            builder.Entity<Vendor>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Vehicle>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<ConsignmentReceipt>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<ConsignmentItem>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<SaleTransaction>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Invoice>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUser = _currentUserService.Email ?? "System";

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(a => a.CreatedAt).CurrentValue = DateTime.UtcNow;
                        if (string.IsNullOrEmpty(entry.Property(a => a.CreatedBy).CurrentValue))
                        {
                            entry.Property(a => a.CreatedBy).CurrentValue = currentUser;
                        }
                        break;
                    case EntityState.Modified:
                        entry.Property(a => a.UpdatedAt).CurrentValue = DateTime.UtcNow;
                        entry.Property(a => a.UpdatedBy).CurrentValue = currentUser;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}