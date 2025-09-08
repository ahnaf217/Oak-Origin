using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    /// <inheritdoc />
    public partial class SeedBlogData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Title", "Slug", "Excerpt", "Content", "CoverImageUrl", "PublishedAt", "CategoryId", "IsPublished" },
                values: new object[,]
                {
                    { 10, "Seeded Post", "seeded-post", "This is a seeded post.", "Seeded content...", "/images/blog/seeded.jpg", DateTime.UtcNow, 1, true }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValues: new object[] { 10 }
            );
        }
    }
}
