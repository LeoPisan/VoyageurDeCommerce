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
        private Stopwatch stopwatch = new Stopwatch();
        public override string Nom => "Christofides";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            // Lancement de la stopwatch
            stopwatch.Start();

            // Lancer FloydWarshall
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // Affecte les lieux et routes dans des listes temporaires et les trie les routes par distances croissantes
            List<Route> listeRouteTemp = new List<Route>(listeRoute.OrderBy(Route => Route.Distance).ToList());
            
            // Arbre couvrant du graphe de base
            List<Route> routesArbreCouvrant = Kruskal(listeLieux, listeRouteTemp);

            // Lieux de degré impair de l'arbre couvrant
            List<Lieu> lieuxDegreImpair = ListeLieuDegreImpair(listeLieux, routesArbreCouvrant);

            // Supprime les routes ne menant plus à aucun lieu et les tris dans l'ordre coissant selon la distance
            List<Route> routeGrapheInduit = SupprimeRouteEnTrop(lieuxDegreImpair, listeRouteTemp).OrderBy(Route => Route.Distance).ToList();

            // Couplage de poids minimum
            List<Route> couplageMinimal = Couplage(routeGrapheInduit, lieuxDegreImpair, routesArbreCouvrant);
                        
            // Union du couplage et de l'arbre couvrant
            List<Route> union = couplageMinimal.Union(routesArbreCouvrant).ToList();

            // Fait un tour eulerien de graphe union
            List<Lieu> final = TourEulerien(listeLieux, union);
            
            // Ajoute dans  la tournée chaque lieu du tour eulerien
            foreach(Lieu lieu in final)
            {
                Tournee.ListeLieux.Add(lieu);

                // Capture de la tournée
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }

            Outils.FinCycle(Tournee);
            stopwatch.Stop();

            // Capture de la tournée
            this.NotifyPropertyChanged("Tournee");

        }



        /// <summary>
        /// Fait un tour eulerien d'un graphe
        /// </summary>
        /// <param name="lieux">Lieux du graphe</param>
        /// <param name="routes">Routes du graphe</param>
        private List<Lieu> TourEulerien(List<Lieu> lieux, List<Route> routes) 
        {            
            // Initialisation
            List<Lieu> voisins;
            List<Lieu> res = new List<Lieu>();  // Création de la liste de résultat
            List<Route> routesTemp = new List<Route>(routes);  // Affecte les valeurs des paramètres dans des variables temporaires

            // Premier lieu
            Lieu lieuTravail = lieux[0];
            res.Add(lieuTravail);
            voisins = Outils.Voisins(lieuTravail, routesTemp);


            // Retire la route entre le lieu actuel et le suivant
            routesTemp.Remove(routesTemp.Find(Route => ((Route.Depart == lieuTravail) && (Route.Arrivee == voisins[0])) || ((Route.Depart == voisins[0]) && (Route.Arrivee == lieuTravail))));



            bool enCours = true;
            while (enCours)
            {
                if (voisins.Count < 1)
                {
                    enCours = false;
                }
                else
                {
                    lieuTravail = voisins[0];
                    res.Add(lieuTravail);
                    voisins = Outils.Voisins(lieuTravail, routesTemp);
                    routesTemp.Remove(routesTemp.Find(Route => ((Route.Depart == lieuTravail) && (Route.Arrivee == voisins[0])) || ((Route.Depart == voisins[0]) && (Route.Arrivee == lieuTravail))));

                }
            }

            HashSet<Lieu> antiDuplicant = new HashSet<Lieu>(res);
            res = antiDuplicant.ToList();
            return res;
        }






        /// <summary>
        /// Renvoie un couplage de poids minimal d'un graphe
        /// </summary>
        /// <param name="routes">Routes du graphe</param>
        /// <param name="lieux">Lieux du graphe</param>
        private List<Route> Couplage(List<Route> routes, List<Lieu> lieux, List<Route> routesArbre)
        {
            List<Route> res = new List<Route>();
            List<Lieu> lieuxTemp = new List<Lieu>(lieux);
            bool debug = true;
            int i = 0;
            while (debug)
            {
                if (lieuxTemp.Count < 2 || i >= routes.Count) { debug = false; }
                else
                {
                    if (!(routesArbre.Contains(routes[i])) && lieuxTemp.Contains(routes[i].Depart) && lieuxTemp.Contains(routes[i].Arrivee))
                    {
                        lieuxTemp.Remove(routes[i].Depart);
                        lieuxTemp.Remove(routes[i].Arrivee);
                        res.Add(routes[i]);
                    }                                  
                    i++;
                }
            }
            return res;
        }



        /// <summary>
        /// Renvoie une liste qui supprime les routes qui ne mène à aucun lieu d'une liste passée en paramètre
        /// </summary>
        /// <param name="lieux"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        private List<Route> SupprimeRouteEnTrop(List<Lieu> lieux, List<Route> routes)
        {
            List<Route> res = new List<Route>();
            foreach (Route route in routes)
            {                
                if (lieux.Contains(route.Depart) && lieux.Contains(route.Arrivee))
                {
                    res.Add(route);
                }
            }
            return res;
        }



        /// <summary>
        /// Renvoie une liste de Lieu ayant un nombre de voisins impair
        /// </summary>
        /// <param name="listeLieux"></param>
        /// <param name="listeRoute"></param>
        private List<Lieu> ListeLieuDegreImpair(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            List<Lieu> res = new List<Lieu>();
            foreach (Lieu lieu in listeLieux)
            {
                if (Outils.NombreVoisins(lieu, listeRoute) % 2 != 0)
                {
                    res.Add(lieu);
                }
            }
            return res;
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