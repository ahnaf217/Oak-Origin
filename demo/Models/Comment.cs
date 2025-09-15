using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]

        public string UserName { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
