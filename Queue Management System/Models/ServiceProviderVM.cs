using System.ComponentModel.DataAnnotations;

namespace Queue_Management_System.Models
{
    public class ServiceProviderVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
        public string Role { get; set; }

        [Display(Name = "Servicepoint Id")]
        public int ServicepointId { get; set; }
    }
}
