using System;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.parseur
{
    /// <summary>Monteur de lieu </summary>
    public class MonteurLieu
    {
        /// <summary>
        /// Crée un lieu à partir du tableau de string obtenu en lisant le fichier 
        /// </summary>
        /// <param name="morceaux">Les 4 morceaux de la ligne correspondant à la ligne</param>
        /// <returns>Le lieu créé</returns>
        public static Lieu Creer(String[] morceaux)
        {
            TypeLieu type;
            if (morceaux[0] == "USINE")
            {
                type = TypeLieu.USINE;
            }
            else
            {
                type = TypeLieu.MAGASIN;
            }
            string nom = morceaux[1];
            int x = (int)Int64.Parse(morceaux[2]);
            int y = (int)Int64.Parse(morceaux[3]);

            return (new Lieu(type, nom, x, y));
        }
    }
}
