using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(400, ErrorMessage = "Title cannot exceed 400 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")] 
        public string Content { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Author { get; set; }
        public string FeatureImagePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public ICollection<Comment> Comments { get; set; }
    }
}
