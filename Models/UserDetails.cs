using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenX.Models
{
    [Table("users")]
    public class UserDetails
    {
        [Key]
        [Column("userid")]
        public Guid UserId { get; set; }

        [Column("username")]
        public string UserName { get; set; } = string.Empty;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
