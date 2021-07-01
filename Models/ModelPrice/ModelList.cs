using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PanamaPrintApp.Models
{
    public class ModelList
    {
        [Key]
        public int ID { get; set; }

        public string ModelListName { get; set; }

        public List<Model> Models { get; set; }

        public ModelList()
        {
            Models = new List<Model>();
        }
    }
}
