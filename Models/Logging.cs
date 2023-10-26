using System.ComponentModel.DataAnnotations;

namespace LerningAsp.Models
{
    public class Logging
    {
        [Required(ErrorMessage = "Не указан Логин")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Не указан Пароль")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
