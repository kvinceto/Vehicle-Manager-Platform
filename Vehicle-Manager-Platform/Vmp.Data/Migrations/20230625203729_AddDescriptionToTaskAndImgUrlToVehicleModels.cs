using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vmp.Data.Migrations
{
    public partial class AddDescriptionToTaskAndImgUrlToVehicleModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelImgUrl",
                table: "Vehicles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Link to example image for the model of this vehicle");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                comment: "Description of the task that has to be performed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelImgUrl",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tasks");
        }
    }
}
