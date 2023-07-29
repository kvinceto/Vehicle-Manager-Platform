﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vmp.Data;

#nullable disable

namespace Vmp.Data.Migrations
{
    [DbContext(typeof(VehicleManagerDbContext))]
    [Migration("20230728231646_AddRolesAndAdmin")]
    partial class AddRolesAndAdmin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Vmp.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("d22b0574-0303-435d-b4b1-8c5e47e6f622"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1f001c65-387b-4dbc-b98d-d6afd1a792f1",
                            Email = "admin@admin.bg",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@ADMIN.BG",
                            NormalizedUserName = "ADMIN@ADMIN.BG",
                            PasswordHash = "AQAAAAEAACcQAAAAEPAEmIosD107NsqZnReDvpHM2vWvFJ3yj82BQAFNa/x1f0H6ZhFoiZ4xaUq5hhEvhg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ADMIN@ADMIN.BG",
                            TwoFactorEnabled = false,
                            UserName = "admin@admin.bg"
                        });
                });

            modelBuilder.Entity("Vmp.Data.Models.CostCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Cost center identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit")
                        .HasComment("Status of the cost center");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the cost center");

                    b.HasKey("Id");

                    b.ToTable("CostCenters");

                    b.HasComment("Department, Construction site or other cost center");
                });

            modelBuilder.Entity("Vmp.Data.Models.DateCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Chech identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date to be reached");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit")
                        .HasComment("Status of the check");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the check");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier of the cretor of this check");

                    b.Property<string>("VehicleNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasComment("Licence plate of the Vehicle corresponding with this check");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleNumber");

                    b.ToTable("DateChecks");

                    b.HasComment("Check to reach a certain date");
                });

            modelBuilder.Entity("Vmp.Data.Models.MileageCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Check identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ExpectedMileage")
                        .HasColumnType("int")
                        .HasComment("Mileage to be reached");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit")
                        .HasComment("Status of the check: Completed or Ongoing");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the check");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier of the cretor of this check");

                    b.Property<string>("VehicleNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasComment("Licence plate of the Vehicle corresponding with this check");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleNumber");

                    b.ToTable("MileageChecks");

                    b.HasComment("Check to reach a certain Mileage");
                });

            modelBuilder.Entity("Vmp.Data.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Owner identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Info")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Additional info for the owner");

                    b.Property<bool>("IsInactive")
                        .HasColumnType("bit")
                        .HasComment("Status of the owner");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the owner");

                    b.HasKey("Id");

                    b.ToTable("Owners");

                    b.HasComment("Owner of vehicles");
                });

            modelBuilder.Entity("Vmp.Data.Models.TaskModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Task identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasComment("Description of the task that has to be performed");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2")
                        .HasComment("Task deadline");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit")
                        .HasComment("Status of the task");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the task");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier of the user corresponding with this task");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");

                    b.HasComment("Task for a specific user");
                });

            modelBuilder.Entity("Vmp.Data.Models.Vehicle", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)")
                        .HasComment("Vehicle licence plate or unique identifier");

                    b.Property<decimal>("FuelCapacity")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Vehicle maximum fuel capacity");

                    b.Property<decimal>("FuelCostRate")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Consumption of fuel, per 100 km or 1 machine hour, for this vehicle");

                    b.Property<decimal>("FuelQuantity")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Vehicle current fuel quantity");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Status of the vehicle");

                    b.Property<string>("Make")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Vehicle make name");

                    b.Property<int>("Mileage")
                        .HasColumnType("int")
                        .HasComment("Vehicle current mileage");

                    b.Property<string>("Model")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Vehicle model name");

                    b.Property<string>("ModelImgUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)")
                        .HasComment("Link to example image for the model of this vehicle");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int")
                        .HasComment("The unique identifier of the owner of this vehicle");

                    b.Property<string>("VIN")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)")
                        .HasComment("Vehicle VIN");

                    b.HasKey("Number");

                    b.HasIndex("OwnerId");

                    b.ToTable("Vehicles");

                    b.HasComment("Vehicle");
                });

            modelBuilder.Entity("Vmp.Data.Models.Waybill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Waybill identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CostCenterId")
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the cost center corresponding with this waybill");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasComment("Date of issue of the waybill");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time of the creation of the waybill");

                    b.Property<decimal>("FuelConsumed")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Fuel consumed for the day for the corresponding vehicle");

                    b.Property<decimal>("FuelLoaded")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Fuel loaded for the day for the corresponding vehicle");

                    b.Property<decimal>("FuelQuantityEnd")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Fuel at the end of the day for the corresponding vehicle");

                    b.Property<decimal>("FuelQuantityStart")
                        .HasColumnType("decimal(14,3)")
                        .HasComment("Fuel at the beginning of the day for the corresponding vehicle");

                    b.Property<string>("Info")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Additional info for this waybill");

                    b.Property<int>("MileageEnd")
                        .HasColumnType("int")
                        .HasComment("Mileage at the end of the day for the corresponding vehicle");

                    b.Property<int>("MileageStart")
                        .HasColumnType("int")
                        .HasComment("Mileage at the beginning of the day for the corresponding vehicle");

                    b.Property<int>("MileageTraveled")
                        .HasColumnType("int")
                        .HasComment("Mileage traveled for the day");

                    b.Property<string>("RouteTraveled")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Traveled route for the day");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier of the user who created this waybill");

                    b.Property<string>("VehicleNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasComment("Licence plate of the vehicle corresponding with this waybill");

                    b.HasKey("Id");

                    b.HasIndex("CostCenterId");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleNumber");

                    b.ToTable("Waybills");

                    b.HasComment("Waybill of a vehicle");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vmp.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vmp.Data.Models.DateCheck", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", "User")
                        .WithMany("DateChecks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vmp.Data.Models.Vehicle", "Vehicle")
                        .WithMany("DateChecks")
                        .HasForeignKey("VehicleNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Vmp.Data.Models.MileageCheck", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", "User")
                        .WithMany("MileageChecks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vmp.Data.Models.Vehicle", "Vehicle")
                        .WithMany("MileageChecks")
                        .HasForeignKey("VehicleNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Vmp.Data.Models.TaskModel", b =>
                {
                    b.HasOne("Vmp.Data.Models.ApplicationUser", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Vmp.Data.Models.Vehicle", b =>
                {
                    b.HasOne("Vmp.Data.Models.Owner", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Vmp.Data.Models.Waybill", b =>
                {
                    b.HasOne("Vmp.Data.Models.CostCenter", "CostCenter")
                        .WithMany("Waybills")
                        .HasForeignKey("CostCenterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vmp.Data.Models.ApplicationUser", "User")
                        .WithMany("Waybills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Vmp.Data.Models.Vehicle", "Vehicle")
                        .WithMany("Waybills")
                        .HasForeignKey("VehicleNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CostCenter");

                    b.Navigation("User");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Vmp.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("DateChecks");

                    b.Navigation("MileageChecks");

                    b.Navigation("Tasks");

                    b.Navigation("Waybills");
                });

            modelBuilder.Entity("Vmp.Data.Models.CostCenter", b =>
                {
                    b.Navigation("Waybills");
                });

            modelBuilder.Entity("Vmp.Data.Models.Owner", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Vmp.Data.Models.Vehicle", b =>
                {
                    b.Navigation("DateChecks");

                    b.Navigation("MileageChecks");

                    b.Navigation("Waybills");
                });
#pragma warning restore 612, 618
        }
    }
}
