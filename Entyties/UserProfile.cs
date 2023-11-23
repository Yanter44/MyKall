using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LerningAsp.Entyties
{
    public class UserProfile
    {  
         [Key]
        [ForeignKey("User")]
        
        public int  Id { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
      
        public int UserId { get; set; }
        public User User { get; set; }
        

        [NotMapped]
        public IFormFile Image { get; set; }
        
      
       
    }
}
