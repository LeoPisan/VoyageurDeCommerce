using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.vue.composants
{
    /// <summary>
    /// Fabrique de ligne à partir de route
    /// </summary>
    class FabriqueLine
    {
        /// <summary>
        /// Construction d'une ligne
        /// </summary>
        /// <param name="route">Route modèle</param>
        /// <param name="minX">Offset X</param>
        /// <param name="minY">Offset Y</param>
        /// <returns>La ligne à afficher</returns>
        public static Line Creer(Route route, int minX, int minY)
        {
            //Dessin de la ligne
            Line ligne = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = DisplaySettings.TailleLieu / 10,
                X1 = DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (route.Depart.X - minX) + DisplaySettings.TailleLieu / 2,
                X2 = DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (route.Arrivee.X - minX) + DisplaySettings.TailleLieu / 2,
                Y1 = DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (route.Depart.Y - minY) + DisplaySettings.TailleLieu / 2,
                Y2 = DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (route.Arrivee.Y - minY) + DisplaySettings.TailleLieu / 2
            };
            //Menu contextuel
            ToolTip tt = new ToolTip
            {
                Content = "Route entre " + route.Depart.Nom + " et " + route.Arrivee.Nom + "\nDistance : " + route.Distance.ToString()
            };
            ligne.ToolTip = tt;
            return ligne;
        }

    }
}
