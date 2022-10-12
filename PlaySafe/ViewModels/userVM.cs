using System.ComponentModel.DataAnnotations;

namespace PlaySafe.ViewModels
{
    public class userVM
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string phoneNum { get; set; }
        public IFormFile photo { get; set; }
    }
}
