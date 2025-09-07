using System.ComponentModel.DataAnnotations;
namespace demo.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(60)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(80)]
        public string Slug { get; set; } = default!;

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}