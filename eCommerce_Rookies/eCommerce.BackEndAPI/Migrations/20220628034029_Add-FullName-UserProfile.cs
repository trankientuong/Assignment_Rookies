using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.BackEndAPI.Migrations
{
    public partial class AddFullNameUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserProfile");
        }
    }
}
