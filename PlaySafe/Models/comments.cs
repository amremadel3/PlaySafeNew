using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class comments
    {
        [Key]
        public Guid id { get; set; }
        [DisplayName("Comment")]
        public string comment { get; set; }
        public Guid userId { get; set; }
        [ForeignKey("userId")]
        public user user { get; set; }
    }
}
