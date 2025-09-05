using Microsoft.AspNetCore.Identity;

namespace demo.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Qty { get; set; }

        public int total { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public Users User
        {
            get; set;
        }
    }
}
