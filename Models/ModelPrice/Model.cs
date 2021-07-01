using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class Model
    {
        [Key]
        public int ID { get; set; }

        // Наименование техники
        public string ModelName { get; set; }

        // Список услуг для данной техники
        public List<Price> Prices { get; set; }

        public Model()
        {
            Prices = new List<Price>();
        }
    }
}
