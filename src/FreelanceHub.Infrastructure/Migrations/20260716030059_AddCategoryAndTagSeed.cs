using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FreelanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndTagSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "category_id", "name" },
                values: new object[,]
                {
                    { 1, "Web Development" },
                    { 2, "Mobile Development" },
                    { 3, "UI / UX Design" },
                    { 4, "Writing" },
                    { 5, "Marketing" }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "tag_id", "name" },
                values: new object[,]
                {
                    { 1, "C#" },
                    { 2, ".NET" },
                    { 3, "React" },
                    { 4, "SQL" },
                    { 5, "Figma" },
                    { 6, "SEO" },
                    { 7, "API" },
                    { 8, "Content Writing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
