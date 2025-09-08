using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace demo.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Excerpt = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Trends", "trends" },
                    { 2, "Living Room", "living-room" },
                    { 3, "Tips", "tips" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "Content", "CoverImageUrl", "Excerpt", "IsPublished", "PublishedAt", "Slug", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Sample content for trends 2024...", "/images/blog/trends-2024.jpg", "Discover the styles defining 2024, from warm neutrals to curvy silhouettes.", true, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "furniture-trends-2024", "Furniture Trends 2024: What's Hot and What's Not" },
                    { 2, 2, "Sample content for sofa guide...", "/images/blog/sofa-guide.jpg", "Comfort, style and durability—what to check before you buy.", true, new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "guide-choosing-perfect-sofa", "The Ultimate Guide to Choosing the Perfect Sofa" },
                    { 3, 3, "Sample content for budget tips...", "/images/blog/budget-tips.jpg", "Smart ways to save without compromising on quality.", true, new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "budget-friendly-furniture-tips", "Budget-Friendly Furniture Shopping Tips" },
                    { 4, 1, "Sample content for dining table...", "/images/blog/dining-table.jpg", "Top styles for 2024 dining tables.", true, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "modern-dining-table-ideas", "Modern Dining Table Ideas" },
                    { 5, 3, "Sample content for maintenance...", "/images/blog/wood-maintenance.jpg", "Care tips for long-lasting wood finish.", true, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "maintain-wooden-furniture", "How to Maintain Wooden Furniture" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Slug",
                table: "Posts",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
