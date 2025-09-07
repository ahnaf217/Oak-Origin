using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class Product 
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Image { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
