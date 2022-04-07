using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.distances
{
    public class FloydWarshall
    {
        /// <summary>Instance du singleton</summary>
        private static FloydWarshall instance;
        public static FloydWarshall Instance
        {
            get {
                if (instance == null) instance = new FloydWarshall();
                return instance;
            }
        }
        /// <summary>Tableau à double entrée qui permettra de stocker les distances</summary>
        private Dictionary<Lieu, Dictionary<Lieu, int>> tableauDistances;
        /// <summary>Tableau à double entrée qui permettra de stocker les prédécesseurs</summary>
        private Dictionary<Lieu, Dictionary<Lieu, Lieu>> tableauPredecesseurs;
        /// <summary>Tableau des routes</summary>
        private Dictionary<Lieu, Dictionary<Lieu, Route>> tableauRoutes;
        private int infini;

        /// <summary>Constructeur privé</summary>
        private FloydWarshall()
        {
            this.tableauDistances = new Dictionary<Lieu, Dictionary<Lieu, int>>();
            this.tableauPredecesseurs = new Dictionary<Lieu, Dictionary<Lieu, Lieu>>();
            this.tableauRoutes = new Dictionary<Lieu, Dictionary<Lieu, Route>>();
        }


        private void Initialiser(List<Lieu> listeDesLieux, List<Route> listeDesRoutes)
        {
            //On calcul l'infini
            this.infini = 1;
            foreach (Route route in listeDesRoutes)
            {
                infini += route.Distance;
            }

            //On met toutes les distances à infini et les prédécesseur à null
            foreach (Lieu lieu1 in listeDesLieux)
            {
                tableauDistances[lieu1] = new Dictionary<Lieu, int>();
                tableauPredecesseurs[lieu1] = new Dictionary<Lieu, Lieu>();
                tableauRoutes[lieu1] = new Dictionary<Lieu, Route>();
                foreach (Lieu lieu2 in listeDesLieux)
                {
                    tableauDistances[lieu1][lieu2] = infini;
                    tableauPredecesseurs[lieu1][lieu2] = null;
                }
            }

            //On met la distance d'un lieu à lui même à 0
            foreach (Lieu lieu1 in listeDesLieux)
                tableauDistances[lieu1][lieu1] = 0;


            //On traite toutes les routes
            foreach (Route route in listeDesRoutes)
            {
                tableauDistances[route.Depart][route.Arrivee] = route.Distance;
                tableauDistances[route.Arrivee][route.Depart] = route.Distance;
                tableauPredecesseurs[route.Depart][route.Arrivee] = route.Depart;
                tableauPredecesseurs[route.Arrivee][route.Depart] = route.Arrivee;
                tableauRoutes[route.Depart][route.Arrivee] = route;
                tableauRoutes[route.Arrivee][route.Depart] = route;
            }
        }

        /// <summary>
        /// Méthode principale lancant le calcul des distances entre toutes les paires de lieux
        /// </summary>
        /// <param name="listeDesLieux">Liste des lieux (sommets) du graphe</param>
        /// <param name="listeDesRoutes">Liste des routes (arêtes) du graphe</param>
        public static void calculerDistances(List<Lieu> listeDesLieux, List<Route> listeDesRoutes)
        {
            Instance.Initialiser(listeDesLieux, listeDesRoutes);

            foreach (Lieu lieuK in listeDesLieux)
                foreach (Lieu lieuI in listeDesLieux)
                    foreach (Lieu lieuJ in listeDesLieux)
                    {
                        if(Instance.tableauDistances[lieuI][lieuJ]> Instance.tableauDistances[lieuI][lieuK] + Instance.tableauDistances[lieuK][lieuJ])
                        {
                            Instance.tableauDistances[lieuI][lieuJ] = Instance.tableauDistances[lieuI][lieuK] + Instance.tableauDistances[lieuK][lieuJ];
                            Instance.tableauPredecesseurs[lieuI][lieuJ] = Instance.tableauPredecesseurs[lieuK][lieuJ];
                        }
                    }
        }

        /// <summary>
        /// Renvoie la distance (si les calculs ont été lancés avant !) entre les deux lieux
        /// </summary>
        /// <param name="depart">Lieu de départ</param>
        /// <param name="arrivee">Lieu d'arrivé</param>
        public static int Distance(Lieu depart, Lieu arrivee)
        {
            return Instance.tableauDistances[depart][arrivee];
        }

        /// <summary>
        /// Renvoie la liste des routes à suivre pour aller de depart à arrivée
        /// </summary>
        /// <param name="depart">Lieu de départ</param>
        /// <param name="arrivee">Lieu d'arrivée</param>
        /// <returns></returns>
        public static List<Route> Chemin(Lieu depart, Lieu arrivee)
        {
            List<Route> chemin = new List<Route>();
            Lieu courant = arrivee;
            while(Instance.tableauPredecesseurs[depart][courant] != null)
            {
                Lieu predecesseur = Instance.tableauPredecesseurs[depart][courant];
                chemin.Add(Instance.tableauRoutes[predecesseur][courant]);
                courant = predecesseur;
            }
            chemin.Reverse();
            return chemin;
        }
    }
}
