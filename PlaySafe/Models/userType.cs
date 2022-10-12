using System.ComponentModel.DataAnnotations;

namespace PlaySafe.Models
{
    public class userType
    {
        [Key]
        public Guid id { get; set; }
        public string usersType { get; set; }
    }
}
