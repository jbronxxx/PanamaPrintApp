using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DisplayName("Название") ]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DisplayName("ИНН")]
        public string INN { get; set; }

        [DisplayName("Адрес Организации")]
        public string Adress { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
