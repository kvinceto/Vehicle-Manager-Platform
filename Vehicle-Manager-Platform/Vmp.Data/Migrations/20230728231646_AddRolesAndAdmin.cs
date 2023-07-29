using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vmp.Data.Migrations
{
    public partial class AddRolesAndAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d22b0574-0303-435d-b4b1-8c5e47e6f622"), 0, "1f001c65-387b-4dbc-b98d-d6afd1a792f1", "admin@admin.bg", true, false, null, "ADMIN@ADMIN.BG", "ADMIN@ADMIN.BG", "AQAAAAEAACcQAAAAEPAEmIosD107NsqZnReDvpHM2vWvFJ3yj82BQAFNa/x1f0H6ZhFoiZ4xaUq5hhEvhg==", null, false, "ADMIN@ADMIN.BG", false, "admin@admin.bg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d22b0574-0303-435d-b4b1-8c5e47e6f622"));
        }
    }
}
