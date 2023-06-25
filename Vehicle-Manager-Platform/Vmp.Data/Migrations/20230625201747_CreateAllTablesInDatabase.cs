using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vmp.Data.Migrations
{
    public partial class CreateAllTablesInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Cost center identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the cost center"),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the cost center")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                },
                comment: "Department, Construction site or other cost center");

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Owner identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the owner"),
                    Info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "Additional info for the owner"),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the owner")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                },
                comment: "Owner of vehicles");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Task identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the task"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Task deadline"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of the user corresponding with this task"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the task")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Task for a specific user");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Number = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, comment: "Vehicle licence plate or unique identifier"),
                    VIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false, comment: "Vehicle VIN"),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Vehicle model name"),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Vehicle make name"),
                    Mileage = table.Column<int>(type: "int", nullable: false, comment: "Vehicle current mileage"),
                    FuelQuantity = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Vehicle current fuel quantity"),
                    FuelCapacity = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Vehicle maximum fuel capacity"),
                    OwnerId = table.Column<int>(type: "int", nullable: false, comment: "The unique identifier of the owner of this vehicle"),
                    FuelCostRate = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Consumption of fuel, per 100 km or 1 machine hour, for this vehicle"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the vehicle")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Vehicles_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Vehicle");

            migrationBuilder.CreateTable(
                name: "DateChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Chech identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the check"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date to be reached"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(12)", nullable: false, comment: "Licence plate of the Vehicle corresponding with this check"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the check"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of the cretor of this check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DateChecks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DateChecks_Vehicles_VehicleNumber",
                        column: x => x.VehicleNumber,
                        principalTable: "Vehicles",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Check to reach a certain date");

            migrationBuilder.CreateTable(
                name: "MileageChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Check identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the check"),
                    ExpectedMileage = table.Column<int>(type: "int", nullable: false, comment: "Mileage to be reached"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(12)", nullable: false, comment: "Licence plate of the Vehicle corresponding with this check"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the check: Completed or Ongoing"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of the cretor of this check")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileageChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageChecks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MileageChecks_Vehicles_VehicleNumber",
                        column: x => x.VehicleNumber,
                        principalTable: "Vehicles",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Check to reach a certain Mileage");

            migrationBuilder.CreateTable(
                name: "Waybills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Waybill identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of issue of the waybill"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(12)", nullable: false, comment: "Licence plate of the vehicle corresponding with this waybill"),
                    MileageStart = table.Column<int>(type: "int", nullable: false, comment: "Mileage at the beginning of the day for the corresponding vehicle"),
                    MileageEnd = table.Column<int>(type: "int", nullable: false, comment: "Mileage at the end of the day for the corresponding vehicle"),
                    MileageTraveled = table.Column<int>(type: "int", nullable: false, comment: "Mileage traveled for the day"),
                    RouteTraveled = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Traveled route for the day"),
                    FuelQuantityStart = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Fuel at the beginning of the day for the corresponding vehicle"),
                    FuelQuantityEnd = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Fuel at the end of the day for the corresponding vehicle"),
                    FuelConsumed = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Fuel consumed for the day for the corresponding vehicle"),
                    FuelLoaded = table.Column<decimal>(type: "decimal(14,3)", nullable: false, comment: "Fuel loaded for the day for the corresponding vehicle"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Status of the waybill"),
                    Info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "Additional info for this waybill"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time of the creation of the waybill"),
                    CostCenterId = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the cost center corresponding with this waybill"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of the user who created this waybill")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waybills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waybills_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Waybills_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Waybills_Vehicles_VehicleNumber",
                        column: x => x.VehicleNumber,
                        principalTable: "Vehicles",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Waybill of a vehicle");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DateChecks_UserId",
                table: "DateChecks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DateChecks_VehicleNumber",
                table: "DateChecks",
                column: "VehicleNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MileageChecks_UserId",
                table: "MileageChecks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageChecks_VehicleNumber",
                table: "MileageChecks",
                column: "VehicleNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Waybills_CostCenterId",
                table: "Waybills",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Waybills_UserId",
                table: "Waybills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Waybills_VehicleNumber",
                table: "Waybills",
                column: "VehicleNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DateChecks");

            migrationBuilder.DropTable(
                name: "MileageChecks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Waybills");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CostCenters");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
