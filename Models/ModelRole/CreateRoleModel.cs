using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
