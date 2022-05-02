using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;


namespace VoyageurDeCommerce.modele.algorithmes
{
    class Outils
    {
        /// <summary>
        /// cherche l'usine d'une liste de lieux
        /// </summary>
        /// <param name="liste">liste dans laquelle l'usine doit être cherchée</param>
        /// <returns>Lieu représentant l'usine</returns>
        public static Lieu UsineDepart(List<Lieu> liste)
        {
            Lieu retour = null;
            foreach (Lieu l in liste)
            {
                if (l.Type == TypeLieu.USINE)
                    retour = l;
            }
            return retour;
        }

        /// <summary>
        /// inverse les indexes de deux éléments d'une liste
        /// </summary>
        /// <param name="a">indexe du premier élément</param>
        /// <param name="b">indexe du deuxième élément</param>
        /// <param name="listeEntree">liste où les deux éléments ont été inversés</param>
        /// <returns>liste où les éléments ont été inversés</returns>
        public static List<Lieu> InverseElements(int a, int b, List<Lieu> listeEntree)
        {
            List<Lieu> liste = new List<Lieu>(listeEntree);
            Lieu temp = liste[a];
            liste[a] = liste[b];
            liste[b] = temp;
            return liste;
        }

        /// <summary>
        /// inverse la position de l'usine d'une liste et de son premier élément
        /// </summary>
        /// <param name="liste">liste à modifier</param>
        /// <returns>nouvelle version de la liste</returns>
        public static List<Lieu> OrganiseUsine(List<Lieu> liste)
        {
            return (InverseElements(liste.IndexOf(UsineDepart(liste)), 0, liste));
        }





        /// <summary>
        /// Renvoie l'index du lieu où aprèes lequel on insère un lieu à une tournee de façon optimale
        /// </summary>
        /// <param name="L"></param>
        /// <param name="T"></param>
        public static int IndexLieuPlusProcheTournee(Lieu L, Tournee T)
        {
            int indexLieu = T.ListeLieux.Count - 1;
            int temp;
            int min = FloydWarshall.DistanceCouple(L, T.ListeLieux[0], T.ListeLieux[T.ListeLieux.Count - 1]);
            for (int i = 0; i < T.ListeLieux.Count - 2; i++)
            {
                temp = FloydWarshall.DistanceCouple(L, T.ListeLieux[i], T.ListeLieux[i + 1]);
                if (temp < min)
                {
                    indexLieu = i;
                    min = temp;
                }
            }
            return indexLieu;
        }


        /// <summary>
        /// Renvoie l'index du lieu où aprèes lequel on insère un lieu à une tournee de façon optimale
        /// </summary>
        /// <param name="L"></param>
        /// <param name="T"></param>
        public static int IndexLieuPlusLoinTournee(Lieu L, Tournee T)
        {
            int indexLieu = T.ListeLieux.Count - 1;
            int temp;
            int max = FloydWarshall.DistanceCouple(L, T.ListeLieux[0], T.ListeLieux[T.ListeLieux.Count - 1]);
            for (int i = 0; i < T.ListeLieux.Count - 2; i++)
            {
                temp = FloydWarshall.DistanceCouple(L, T.ListeLieux[i], T.ListeLieux[i + 1]);
                if (temp > max)
                {
                    indexLieu = i;
                    max = temp;
                }
            }
            return indexLieu;
        }

        /// <summary>
        /// ajoute le premier élément d'une tournée à la fin de celle-ci (on économise trois secondes de réflexions, c'est bokou)
        /// </summary>
        /// <param name="tourne">liste à réorganiser</param>
        public static void FinCycle(Tournee tournee)
        {
            tournee.Add(tournee.ListeLieux[0]);
        }


        /// <summary>
        /// Renvoie la liste des voisins d'un point
        /// </summary>
        /// <param name="L">Lieu dont on veut les voisins</param>
        /// <param name="listeRoutes">Routes du graphe</param>
        public static List<Lieu> Voisins(Lieu L, List<Route> listeRoutes)
        {
            // Création de la liste de résultat
            List<Lieu> res = new List<Lieu>();

            // Boucle testant toutes routes du paramètre
            foreach (Route route in listeRoutes)
            {
                // Ajoute la route actuelle s'elle est voisine de L
                if (route.Depart == L)
                {
                    res.Add(route.Arrivee);
                }
                // Ajoute la route actuelle s'elle est voisine de L
                if (route.Arrivee == L)
                {
                    res.Add(route.Depart);
                }
            }

            // Retourne le résultat
            return res;
        }


        /// <summary>
        /// Renvoie True si le lieu L possède au moins 1 voisin
        /// </summary>
        /// <param name="L">Lieu dont lequel on veut s'avoir s'il possède au moins 1 voisin</param>
        /// <param name="listeRoutes">Routes du graphe</param>
        public static bool EstVoisin(Lieu L, List<Route> listeRoutes)
        {
            // Création du booléen de rsultat
            bool res = false;

            // Varaible utiles 
            int i = 0;  // Indentation
            bool verif = true;  // Fin de boucle

            // Boucle s'arrêtant après avoir trouvé un voisnin ou avoir testées toutes les routes possibles
            while (verif)
            {
                // Si toutes les routes ont été testées
                if (i > listeRoutes.Count - 1)
                {
                    verif = false;
                }
                // Si un voisin a été trouvé
                else if (listeRoutes[i].Depart == L)
                {
                    verif = false;
                    res = true;
                }
                // Si un voisin a été trouvé
                else if (listeRoutes[i].Arrivee == L)
                {
                    verif = false;
                    res = true;
                }
                // Indente i dans tous les cas pour passer au voisin suivant
                i++;
            }

            // Retourne le résultat
            return res;
        }



        /// <summary>
        /// Renvoie le nombre de voisin d'un point
        /// </summary>
        /// <param name="L">Lieu dont on veut le nombre de voisins</param>
        /// <param name="listeRoutes">Routes du graphe</param>
        public static int NombreVoisins(Lieu L, List<Route> listeRoutes)
        {
            int res = 0;
            foreach (Route route in listeRoutes)
            {
                if (route.Depart == L)
                {
                    res++;
                }
                if (route.Arrivee == L)
                {
                    res++;
                }
            }
            return res;
        }



        public static void AfficheRoute(List<Route> routes)
        {
            foreach (Route route in routes)
            {
                Console.WriteLine("Route : " + route.Depart.ToString() + " à " + route.Arrivee.ToString());
            }
        }


        public static void AfficheLieu(List<Lieu> lieux)
        {
            Console.Write("Lieu : ");
            foreach (Lieu lieu in lieux)
            {
                Console.Write(lieu.ToString() + " - ");
            }
            Console.WriteLine();
        }



    }
}
