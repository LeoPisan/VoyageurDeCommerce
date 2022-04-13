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
            List<Lieu> lieuxArbreCouvrant = new List<Lieu>();
            List<Route> routesArbreCouvrant = new List<Route>();

            Lieu lieuPrincipal;
            List<Lieu> voisins;

            Dictionary<Lieu, int> cout = new Dictionary<Lieu, int>();
            foreach (Lieu lieu in listeLieuxTemp)
            {
                //cout.Add(lieu, FloydWarshall.Infini);
            }

            while (listeLieuxTemp.Count > 0)
            {
                lieuPrincipal = listeLieuxTemp[0];
                listeLieuxTemp.Remove(lieuPrincipal);
                voisins = Outils.Voisins(lieuPrincipal, listeRouteTemp);
                foreach (Lieu lieu in voisins)
                {
                    
                }
            }

    

        }
    }
}