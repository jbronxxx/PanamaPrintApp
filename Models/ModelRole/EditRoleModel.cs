using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class EditRoleModel
    {
        public string ID { get; set; }

        [Required(ErrorMessage ="Обязательно для заполнения")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
