using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.vue.ressources;

namespace VoyageurDeCommerce.vue.composants
{   
    /// <summary>
    /// Image d'un lieu
    /// </summary>
    class ImageLieu : Image
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="lieu">Le lieu</param>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        public ImageLieu(Lieu lieu,int minX,int minY)
        {
            //Source et taille
            this.Source = ImagesManager.GetImage(lieu.Type.ToString());
            this.Height = DisplaySettings.TailleLieu;
            //Position
            Canvas.SetZIndex(this, 2);
            Canvas.SetLeft(this, DisplaySettings.TailleLieu* DisplaySettings.FacteurEspacement * (lieu.X-minX));
            Canvas.SetTop(this, DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (lieu.Y-minY));
            //Tooltips
            ToolTip tt = new ToolTip
            {
                Content = lieu.Nom + "\nType : " + lieu.Type.ToString()
            };
            this.ToolTip = tt;

        }        
    }
}
