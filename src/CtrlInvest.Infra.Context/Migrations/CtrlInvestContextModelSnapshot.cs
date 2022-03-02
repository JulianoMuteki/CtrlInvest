﻿// <auto-generated />
using System;
using CtrlInvest.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CtrlInvest.Infra.Context.Migrations
{
    [DbContext(typeof(CtrlInvestContext))]
    partial class CtrlInvestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CtrlInvest.Domain.Entities.HistoricalPrice", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TickerCode")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<double>("AdjClose")
                        .HasColumnType("double precision");

                    b.Property<double>("Close")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("High")
                        .HasColumnType("double precision");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<double>("Low")
                        .HasColumnType("double precision");

                    b.Property<double>("Open")
                        .HasColumnType("double precision");

                    b.Property<Guid>("TickerID")
                        .HasColumnType("uuid");

                    b.Property<int>("Volume")
                        .HasColumnType("integer");

                    b.HasKey("Date", "TickerCode");

                    b.HasIndex("TickerID");

                    b.ToTable("HistoricalPrices");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.StocksExchanges.Earning", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateWith")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("TickerID")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<double>("ValueIncome")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("TickerID");

                    b.ToTable("Earnings");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Exchange")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.TicketSync", b =>
                {
                    b.Property<Guid>("TickerID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("TicketSyncID");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.HasKey("TickerID", "Id");

                    b.ToTable("TicketSyncs");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.HistoricalPrice", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Entities.Ticket", "Ticket")
                        .WithMany("HistoricalPrices")
                        .HasForeignKey("TickerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.StocksExchanges.Earning", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Entities.Ticket", "Ticket")
                        .WithMany("Earnings")
                        .HasForeignKey("TickerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.TicketSync", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Entities.Ticket", "Ticket")
                        .WithMany("TicketSyncs")
                        .HasForeignKey("TickerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationRoleClaim", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserClaim", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserLogin", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUserToken", b =>
                {
                    b.HasOne("CtrlInvest.Domain.Identity.ApplicationUser", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Entities.Ticket", b =>
                {
                    b.Navigation("Earnings");

                    b.Navigation("HistoricalPrices");

                    b.Navigation("TicketSyncs");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("CtrlInvest.Domain.Identity.ApplicationUser", b =>
                {
                    b.Navigation("UserClaims");

                    b.Navigation("UserLogins");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
