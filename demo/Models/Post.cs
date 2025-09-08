#nullable enable

using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; } = default!;

        [Required, MaxLength(180)]
        public string Slug { get; set; } = default!; // /blog/{slug}

        [MaxLength(220)]
        public string? Excerpt { get; set; }  // listing info

        [Required]
        public string Content { get; set; } = default!; // details content (HTML/plain)

        [MaxLength(250)]
        public string? CoverImageUrl { get; set; } // /images/blog/sofa.jpg

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        // Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}