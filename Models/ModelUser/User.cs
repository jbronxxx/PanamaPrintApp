using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class User
    {
        [Key]
        public string ID { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
