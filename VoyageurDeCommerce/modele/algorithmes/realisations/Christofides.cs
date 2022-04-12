using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;
using System.Diagnostics;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Exemple de classe Algorithme, à ne pas garder
    /// </summary>
    public class Christofides : Algorithme
    {

        public override string Nom => "Christofides";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            //Lancer FloydWarshall
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // Affecte les lieux et routes dans des listes temporaires
            List<Lieu> listeLieuxTemp = listeLieux;
            List<Route> listeRouteTemp = listeRoute;
            
            //
            Dictionary<Lieu, int> coutDesLieux;
            Dictionary<Lieu, Lieu> predDesLieux;            
            foreach (Lieu lieu in listeLieuxTemp)
            {
                //coutDesLieux.Add(lieu, FloydWarshall.Infini);
            }



        }
    }
}