using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VoyageurDeCommerce.vue.ressources
{
    /// <summary>
    /// Gestionnaire d'images
    /// </summary>
    class ImagesManager
    {
        private readonly Dictionary<String, BitmapImage> images;
        private static ImagesManager instance;

        private ImagesManager()
        {
            //Initialisation
            this.images = new Dictionary<string, BitmapImage>();
            //Ajout des images
            //MUR
            this.AjouterImage("MAGASIN", "shop.png");
            this.AjouterImage("USINE", "factory.png");
            this.AjouterImage("TOURNEE", "truck.png");
            this.AjouterImage("ICON", "icon.ico");
        }

        /// <summary>
        /// Ajoute une image au gestionnaire
        /// </summary>
        /// <param name="nom">Nom de l'image</param>
        /// <param name="path">Nom du fichier image</param>
        private void AjouterImage(String nom, String path)
        {
            this.images[nom] = new BitmapImage(new Uri("pack://application:,,,/vue/ressources/images/" + path));
        }

        /// <summary>
        /// Getter du singleton
        /// </summary>
        /// <returns>L'instance du singleton</returns>
        private static ImagesManager Get()
        {
            if (instance == null) instance = new ImagesManager();
            return instance;
        }

        /// <summary>
        /// Getter d'une image
        /// </summary>
        /// <param name="nom">Nom de l'image désirée</param>
        /// <returns>L'image désirée</returns>
        public static BitmapImage GetImage(String nom)
        {
            return Get().images[nom];
        }

    }
}
