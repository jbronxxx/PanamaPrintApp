using System.Collections.Generic;

namespace PanamaPrintApp.Models.ModelPrice
{
    public class Model
    {
        // Наименование техники
        public string ModelName { get; set; }

        // Список услуг для данной техники
        public List<Price> prices { get; set; }

        public Model()
        {
            prices = new List<Price>();
        }
    }
}
