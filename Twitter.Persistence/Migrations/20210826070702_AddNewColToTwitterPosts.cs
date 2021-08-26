using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Persistence.Migrations
{
    public partial class AddNewColToTwitterPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NComments",
                table: "TwitterPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NComments",
                table: "TwitterPosts");
        }
    }
}
