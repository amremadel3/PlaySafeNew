using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class player
    {
        [Key]
        public Guid id { get; set; }
        public Guid userId { get; set; }
        [ForeignKey("userId")]
        public user user { get; set; }
        public Guid adminId { get; set; }
        [Required]
        public string pic { get; set; }
        public int points { get; set; } = 0;

    }
}
