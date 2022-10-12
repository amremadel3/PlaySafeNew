using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class matchHistory
    {
        [Key]
        public Guid id { get; set; }
        public Guid userId { get; set; }
        [ForeignKey("userId")]
        public user user { get; set; }
        public Guid entryId { get; set; }
        [ForeignKey("entryId")]
        public entry entry { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
        public bool active { get; set; }
        public bool withPoints { get; set; }
    }
    public class matchViewModel 
    {
        public int matchCost { get; set; }
        public bool withPoints { get; set; }
    }
}
