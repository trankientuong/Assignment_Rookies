using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.BackEndAPI.Migrations
{
    public partial class ChangeColumnNameUserIdToAccountIdProductRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_UserProfile_UserId",
                table: "ProductRatings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductRatings",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_UserId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_UserProfile_AccountId",
                table: "ProductRatings",
                column: "AccountId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_UserProfile_AccountId",
                table: "ProductRatings");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ProductRatings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_AccountId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_UserProfile_UserId",
                table: "ProductRatings",
                column: "UserId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
