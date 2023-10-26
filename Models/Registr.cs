using System.ComponentModel.DataAnnotations;

namespace LerningAsp.Models
{
    public class Registr
    {
        [Required(ErrorMessage = "Не указан Логин")]
        [Display(Name ="Login")]
       
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан Еmail")]
        [Display(Name = "Email")]
        
        public string EMail { get; set; }
        [Required(ErrorMessage = "Не указан Пароль")]
        [Display(Name = "Password")]    
        public string Password { get; set; }

    }
}
