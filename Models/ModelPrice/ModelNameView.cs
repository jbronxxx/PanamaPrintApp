using System.Collections.Generic;

namespace PanamaPrintApp.Models
{
    public class ModelNameView
    {
        public string Title { get; set; }

        public List<ModelList> ModelNames { get; set; }

        public ModelNameView()
        {
            ModelNames = new List<ModelList>(); 
        }
    }
}
