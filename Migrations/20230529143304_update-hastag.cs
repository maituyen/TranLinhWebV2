using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    public partial class updatehastag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Hastags",
                type: "int",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Hastags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Src",
                table: "Hastags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Hastags",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Hastags");

            migrationBuilder.DropColumn(
                name: "Src",
                table: "Hastags");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Hastags");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Hastags",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
