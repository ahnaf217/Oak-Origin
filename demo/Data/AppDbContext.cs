using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using demo.Models;

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

        public DbSet<Category> Categories { get; set; }

        public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<RecyclingRequest> RecyclingRequests { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; } 

        

    }
    
    
}
