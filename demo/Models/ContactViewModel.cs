using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
