using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public DateTime Date { get; set; }

        [Display(Name = "Наименование Техники")]
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string EquipmentName { get; set; }

        [Display(Name = "Выполненные работы")]
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string OrderName { get; set; }

        // Расходные материалы
        [Display(Name = "Расходные материалы")]
        public string Consumables { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
