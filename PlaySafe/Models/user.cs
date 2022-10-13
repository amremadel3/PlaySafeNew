using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PlaySafe.Models
{
    public class user
    {
        [Key]
        public Guid id { get; set; }
        public Guid userTypeId { get; set; }
        [ForeignKey("userTypeId")]
        public userType userType { get; set; }
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }
        [Required]
        [DisplayName("Username")]
        public string userName { get; set; }
        [Required]
        [DisplayName("Password")]
        public byte[] password { get; set; }
        [DisplayName("Account Creatation Date")]
        public DateTime createdDate { get; set; } = DateTime.Now;
        [DisplayName("Phone Num")]
        public string phoneNum { get; set; }
        public string? photo { get; set; }
        public int? points { get; set; } = 0;
    }
    public class registerViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string phoneNum { get; set; }
        public IFormFile? photo { get; set; }
    }

    public class loginViewModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }   
}
