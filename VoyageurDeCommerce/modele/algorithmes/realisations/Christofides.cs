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
            List<Route> listeRouteTemp = listeRoute.OrderBy(Route => Route.Distance).ToList(); // Trie les routes par distances croissantes
            
            List<Route> routesArbreCouvrant = Kruskal(listeLieuxTemp, listeRouteTemp);
        }


        /// <summary>
        /// Algorithme de Kruskal qui renvoit les routes de l'arbre couvrant d'un graphe passé en paramètre via les lieux et routes
        /// </summary>
        /// <param name="lieux"></param>
        /// <param name="routes"></param>
        private List<Route> Kruskal(List<Lieu> lieux, List<Route> routes)
        {
            List<Route> res = new List<Route>();

            // Initialise une composante connexe pour chaque lieux
            Dictionary<Lieu, int> listeComposanteConnexe = new Dictionary<Lieu, int>();
            int i = 1;
            foreach (Lieu lieu in lieux)
            {
                listeComposanteConnexe.Add(lieu, i);
                i++;
            }

            // Initialisations de variables utiles
            Lieu lieu1;
            Lieu lieu2;
            int changement;

            // Algorithme de Kruskal
            foreach (Route route in routes)
            {
                lieu1 = route.Depart;
                lieu2 = route.Arrivee;
                if (listeComposanteConnexe[lieu1] != listeComposanteConnexe[lieu2])
                {
                    // On ajoute à notre arbre couvrant la route travaillée
                    res.Add(route);

                    // On fusionne les composantes connexes
                    changement = listeComposanteConnexe[lieu2];
                    foreach (Lieu lieu in lieux)
                    {
                        if (listeComposanteConnexe[lieu] == changement)
                        {
                            listeComposanteConnexe[lieu] = listeComposanteConnexe[lieu1];
                        }
                    }
                }
            }
            return res;
        }


   
    }
}