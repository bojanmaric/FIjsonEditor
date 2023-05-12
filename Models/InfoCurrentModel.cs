
using System.Collections.Generic;

namespace EditJsonFInspection.Models
{
    public class InfoCurrentModel
    {
        public List<string> ProdHierarchies { get; set; }
        public InfoMessageModel InfoMessage { get; set; }
        public string KeyShowMessagePosition { get; set; }
        public int OrderInfoMessage { get; set; }

        public int WorkPlaceType { get; set; }

        // Added new variable for place name
        public string PictureFile { get; set; }

        // Mistake war with Letter in name, to left with Mistake or?
        public List<ButtonConfigurationModel> ButtonsConfigruation { get; set; }
    }
}
