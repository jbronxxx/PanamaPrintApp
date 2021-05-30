using System.ComponentModel.DataAnnotations;

namespace CompanyData.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
