using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LerningAsp.Entyties
{
    public class UserProfileImage
    {
        [Key]
        [ForeignKey("User")]
        public int  Id { get; set; }
        public byte[] Avatar { get; set; }
        public User User { get; set; }
    }
}
