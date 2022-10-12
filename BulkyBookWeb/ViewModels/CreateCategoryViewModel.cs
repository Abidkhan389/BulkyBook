using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.ViewModels
{
    public class CreateCategoryViewModel
    {
        
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1,500, ErrorMessage ="Order must be between 1 to 500 only")]
        public int Displayorder { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
