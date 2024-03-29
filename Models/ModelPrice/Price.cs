﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class Price
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DisplayName("Наименование")]
        public string PriceName { get; set; }
 
        [DisplayName("Цена")]
        public string ServicePrice { get; set; }

        public int ModelID { get; set; }
        public Model Model { get; set; }
    }
}
