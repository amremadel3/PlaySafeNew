using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class userTypePages
    {
        public Guid id { get; set; }
        public Guid userTypeId { get; set; }
        [ForeignKey("userTypeId")]
        public userType userType { get; set; }
        public int page { get; set; }
    }
}
