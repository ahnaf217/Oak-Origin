using Microsoft.AspNetCore.Identity;
namespace demo.Models
{
    public class Address
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public Users User { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PinCode { get; set; }

    }
}
