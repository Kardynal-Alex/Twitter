using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Persistence.Migrations
{
    public partial class AddNewColumnToComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TwitterPostId",
                table: "Comments",
                column: "TwitterPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_TwitterPosts_TwitterPostId",
                table: "Comments",
                column: "TwitterPostId",
                principalTable: "TwitterPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_TwitterPosts_TwitterPostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TwitterPostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Comments");
        }
    }
}
