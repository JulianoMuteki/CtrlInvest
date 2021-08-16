using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Identity;
using CtrlInvest.Infra.Context.Mapping;
using CtrlInvest.Infra.Context.Mapping.Financial;
using CtrlInvest.Infra.Context.Mapping.FinancialClassification;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CtrlInvest.Infra.Context
{
    public class CtrlInvestContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim,
                                                          ApplicationUserRole, ApplicationUserLogin,
                                                          ApplicationRoleClaim, ApplicationUserToken>
    {

        public DbSet<Composite> Composites { get; set; }
        public DbSet<Leaf> Leaves { get; set; }
        public DbSet<ParentTree> ParentsTrees { get; set; }
        public DbSet<ChildTree> ChildrenTrees { get; set; }
        public DbSet<GrandChildTree> GrandChildrenTrees { get; set; }
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
        public DbSet<Bank> Banks { get; set; }

        public CtrlInvestContext()
        {
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public CtrlInvestContext(DbContextOptions<CtrlInvestContext> options)
             : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            ChangeTracker.LazyLoadingEnabled = true;
        }

        public void SetTrackAll()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        public void EnableLazyLoading()
        {
            ChangeTracker.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationFailure>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfiguration(new CompositeMap());
            modelBuilder.ApplyConfiguration(new LeafMap());
            modelBuilder.ApplyConfiguration(new BankMap());

            modelBuilder.ApplyConfiguration(new ParentTreeMap());
            modelBuilder.ApplyConfiguration(new ChildTreeMap());
            modelBuilder.ApplyConfiguration(new GrandChildTreeMap());
            modelBuilder.ApplyConfiguration(new FinancialTransactionMap());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
                {
                // Each User can have many UserClaims
                b.HasMany(e => e.UserClaims)
                        .WithOne(e => e.User)
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.UserLogins)
                            .WithOne(e => e.User)
                            .HasForeignKey(ul => ul.UserId)
                            .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.UserTokens)
                            .WithOne(e => e.User)
                            .HasForeignKey(ut => ut.UserId)
                            .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                            .WithOne(e => e.User)
                            .HasForeignKey(ur => ur.UserId)
                            .IsRequired();

                });
            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                                .WithOne(e => e.Role)
                                .HasForeignKey(rc => rc.RoleId)
                                .IsRequired();
            });
        }
    }
}
