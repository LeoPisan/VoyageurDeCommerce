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

        private static int infini;

        public static int Infini => infini;



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
            infini = 1;
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
        /// Renvoie la distance entre un lieu et un couple de lieu
        /// L.distance(A, B) = A.distance(L) + L.distance(B) − A.distance(B)
        /// </summary>
        /// <param name="L"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int DistanceCouple(Lieu L, Lieu A, Lieu B)
        {
            return Distance(A, L) + Distance(L, B) - Distance(A, B);
        }


        /// <summary>
        /// Renvoie la distance entre un lieu et une tournée
        /// L.distance(T) = M in(L.distance(A, B), L.distance(B, C), L.distance(C, D), L.distance(D, A)
        /// </summary>
        /// <param name="L"></param>
        /// <param name="T"></param>
        public static int DistanceTournee(Lieu L, Tournee T)
        {
            int temp;
            int min = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[1]);
            for (int i = 1; i < T.ListeLieux.Count - 1; i++)
            {
                temp = FloydWarshall.Distance(T.ListeLieux[i], T.ListeLieux[i + 1]);
                if (temp < min)
                {
                    min = temp;
                }
            }
            temp = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[T.ListeLieux.Count - 1]);
            if (temp < min)
            {
                min = temp;
            }
            return min;
        }


        
        /// <summary>
        /// Renvoie le lieu le plus loin au point A
        /// </summary>
        /// <param name="A"></param>
        /// <param name="listeLieux"></param>
        public static Lieu PlusLoin(Lieu A, List<Lieu> listeLieux)
        {
            Lieu max = listeLieux[0];
            foreach (Lieu lieu in listeLieux)
            {
                if (Distance(A, lieu) > Distance(A, max))
                {
                    max = lieu;
                }
            }
            return max;
        }


        /// <summary>
        /// Renvoie les 2 lieux les plus loin l'un de l'autre dans une liste de lieu donnée
        /// </summary>
        /// <param name="listeLieux">Liste de lieu donnée</param>
        public static Lieu[] PlusLoin(List<Lieu> listeLieux)
        {
            Lieu[] couple = new Lieu[2];
            couple[0] = listeLieux[0];
            couple[1] = listeLieux[1];
            foreach (Lieu lieu1 in listeLieux)
            {
                foreach (Lieu lieu2 in listeLieux)
                {
                    if (Distance(lieu1, lieu2) > Distance(couple[0], couple[1]))
                    {
                        couple[0] = lieu1;
                        couple[1] = lieu2;
                    }
                }
            }
            Console.WriteLine(couple[0].ToString());
            Console.WriteLine(couple[1].ToString());
            return couple;
        }


        /// <summary>
        /// Renvoie la distance entre deux lieux voisins
        /// </summary>
        /// <param name="depart">Lieu de départ</param>
        /// <param name="arrivee">Lieu d'arrivée</param>
        /// <param name="routes">Liste des routes</param>
        /// <returns></returns>
        public static int DistanceRoute(Lieu depart, Lieu arrivee, List<Route> routes)
        {
            int res = infini;
            foreach (Route route in routes)
            {
                if ((route.Depart == depart && route.Arrivee == arrivee) || (route.Depart == arrivee && route.Arrivee == depart))
                {
                    res = route.Distance;
                }
            }
            return res;
        }



    }
}
