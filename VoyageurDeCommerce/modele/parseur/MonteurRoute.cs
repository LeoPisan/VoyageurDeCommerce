using System;
using System.Collections.Generic;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.parseur
{
    /// <summary>Monteur des routes</summary>
    public class MonteurRoute
    {
        /// <summary>
        /// Crée une route à partir du tableau de string obtenu en lisant le fichier et de la liste des lieux
        /// </summary>
        /// <param name="morceaux">Les 4 morceaux de la ligne correspondant à la ligne</param>
        /// <param name="listLieux">Liste des lieux indexés par leur nom</param>
        /// <returns>La route créé</returns>
        public static Route Creer(String[] morceaux,Dictionary<String,Lieu> listLieux)
        {
            Lieu l1 = listLieux[morceaux[1]];
            Lieu l2 = listLieux[morceaux[2]];
            int distance = (int)Int64.Parse(morceaux[3]);
            return (new Route(l1, l2, distance));
        }
    }
}
