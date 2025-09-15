using System.ComponentModel;

namespace demo.Models
{
    public class SliderImage
    {
        public int Id { get; set; }

        public string Image { get; set; }

        [DisplayName("Sort Order")]

        public int SortOrder { get; set; }
    }
}
