namespace demo.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        // Foreign key
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

