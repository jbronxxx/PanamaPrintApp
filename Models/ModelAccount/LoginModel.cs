using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
    }
}
