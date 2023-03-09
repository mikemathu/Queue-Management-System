using Queue_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Queue_Management_System.Models
{
    public class ServicePointVM
    {
        public int Id { get; set; }

        [Display(Name = "Service Point Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Service Provider Id")]
        [Required]
        public int ServiceProviderId { get; set; }
    }
}