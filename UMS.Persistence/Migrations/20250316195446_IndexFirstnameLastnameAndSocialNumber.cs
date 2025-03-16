using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IndexFirstnameLastnameAndSocialNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Firstname_Lastname",
                table: "Users",
                columns: new[] { "Firstname", "Lastname" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SocialNumber",
                table: "Users",
                column: "SocialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Firstname_Lastname",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SocialNumber",
                table: "Users");
        }
    }
}
