using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class player : user
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        public string photo { get; set; }
        public int points { get; set; } = 0;
    }
    public class playerViewModel
    {
        public Guid id { get; set; }
        public Guid supervisorId { get; set; }
        public Guid userId { get; set; }
        [Required]
        [DisplayName("Phone Num")]
        public string phoneNum { get; set; }
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }
        [Required]
        [DisplayName("Username")]
        public string userName { get; set; }
        [Required]
        [DisplayName("Password")]
        public string password { get; set; }
        [DisplayName("Confirm Password")]
        public string confirmPassword { get; set; }
        public IFormFile photo { get; set; }
    }

}
