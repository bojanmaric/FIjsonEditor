using Prism.Mvvm;

using System.Collections.Generic;

namespace EditJsonFInspection.Models
{
    public class InfoTextModel : BindableBase
    {
        // My proposal to instead of string put object with contain name and type of scale?
        public List<ProdHierarchiesModel> ProdHierarchies { get; set; }
        public InfoMessageModel InfoMessage { get; set; }
     
        public string KeyShowMessagePosition { get; set ; }
        public int OrderInfoMessage { get; set; }

        public int WorkPlaceType { get; set; }

        // Added new variable for place name
        public string WorkPlaceName { get; set; }
        public string PictureFile { get; set; }

        // Mistake war with Letter in name, to left with Mistake or?
        public List<ButtonConfigurationModel> ButtonsConfiguration { get; set; }
/*
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }*/
    }
}
