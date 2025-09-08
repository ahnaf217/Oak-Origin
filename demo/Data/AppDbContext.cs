using demo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace demo.Data
{ 

    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Index
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            // Seed Category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Trends", Slug = "trends" },
                new Category { Id = 2, Name = "Living Room", Slug = "living-room" },
                new Category { Id = 3, Name = "Tips", Slug = "tips" }
            );

            // Seed Post
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    Title = "Furniture Trends 2024: What's Hot and What's Not",
                    Slug = "furniture-trends-2024",
                    Excerpt = "Discover the styles defining 2024, from warm neutrals to curvy silhouettes.",
                    Content = "Sample content for trends 2024...",
                    CoverImageUrl = "/images/blog/trends-2024.jpg",
                    PublishedAt = new DateTime(2024, 4, 15),
                    CategoryId = 1,
                    IsPublished = true
                },
                new Post
                {
                    Id = 2,
                    Title = "The Ultimate Guide to Choosing the Perfect Sofa",
                    Slug = "guide-choosing-perfect-sofa",
                    Excerpt = "Comfort, style and durability—what to check before you buy.",
                    Content = "Sample content for sofa guide...",
                    CoverImageUrl = "/images/blog/sofa-guide.jpg",
                    PublishedAt = new DateTime(2024, 4, 14),
                    CategoryId = 2,
                    IsPublished = true
                },
                new Post
                {
                    Id = 3,
                    Title = "Budget-Friendly Furniture Shopping Tips",
                    Slug = "budget-friendly-furniture-tips",
                    Excerpt = "Smart ways to save without compromising on quality.",
                    Content = "Sample content for budget tips...",
                    CoverImageUrl = "/images/blog/budget-tips.jpg",
                    PublishedAt = new DateTime(2024, 4, 8),
                    CategoryId = 3,
                    IsPublished = true
                },
                new Post
                {
                    Id = 4,
                    Title = "Modern Dining Table Ideas",
                    Slug = "modern-dining-table-ideas",
                    Excerpt = "Top styles for 2024 dining tables.",
                    Content = "Sample content for dining table...",
                    CoverImageUrl = "/images/blog/dining-table.jpg",
                    PublishedAt = new DateTime(2024, 5, 1),
                    CategoryId = 4,
                    IsPublished = true
                },
                new Post
                {
                    Id = 5,
                    Title = "How to Maintain Wooden Furniture",
                    Slug = "maintain-wooden-furniture",
                    Excerpt = "Care tips for long-lasting wood finish.",
                    Content = "Sample content for maintenance...",
                    CoverImageUrl = "/images/blog/wood-maintenance.jpg",
                    PublishedAt = new DateTime(2024, 5, 5),
                    CategoryId = 5,
                    IsPublished = true
                }
            );

            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.Post)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(c => c.PostId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
    
    
    

