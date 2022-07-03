using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.BackEndAPI.Migrations
{
    public partial class updatecolumntableProductRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_UserProfile_AccountId",
                table: "ProductRatings");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "ProductRatings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_AspNetUsers_AccountId",
                table: "ProductRatings",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_AspNetUsers_AccountId",
                table: "ProductRatings");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "ProductRatings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_UserProfile_AccountId",
                table: "ProductRatings",
                column: "AccountId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
