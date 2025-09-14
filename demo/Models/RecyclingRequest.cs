namespace demo.Models
{
    public class RecyclingRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to your user table
        public int ProductId { get; set; } // Foreign key to your product table
        public string CustomerName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string PickupAddress { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // e.g., "Pending", "Approved", "Completed"
        public decimal StoreCreditAmount { get; set; } // The value of the store credit offered
        public string StoreCreditCode { get; set; } // A unique code for the store credit
        public bool IsCreditUsed { get; set; } = false;

        // Navigation properties
        public Users User { get; set; } // Assuming your user model is ApplicationUser
        public Product Product { get; set; }
    }
}
