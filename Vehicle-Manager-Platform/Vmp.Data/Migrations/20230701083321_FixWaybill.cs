using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vmp.Data.Migrations
{
    public partial class FixWaybill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Waybills");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Waybills",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Status of the waybill");
        }
    }
}
