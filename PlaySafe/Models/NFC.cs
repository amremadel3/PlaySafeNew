using System.ComponentModel.DataAnnotations.Schema;

namespace PlaySafe.Models
{
    public class NFC
    {
        public Guid id { get; set; }
        public Guid userId { get; set; }
        [ForeignKey("userId")]
        public user user { get; set; }

    }
}
