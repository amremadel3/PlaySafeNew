using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class specials
    {
        [Key]
        public Guid id { get; set; }
        public Guid adminId { get; set; }
        [ForeignKey("adminId")]
        public userType userType { get; set; }
        public string special { get; set; }

    }
}
