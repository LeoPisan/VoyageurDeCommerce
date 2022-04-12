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
        /// <returns></returns>
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
            int indexLieu = 1;
            int temp;
            int min = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[1]);
            for (int i = 1; i < T.ListeLieux.Count - 1; i++)
            {
                temp = FloydWarshall.Distance(T.ListeLieux[i], T.ListeLieux[i + 1]);
                if (temp < min)
                {
                    indexLieu = i;
                    min = temp;
                }
            }
            temp = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[T.ListeLieux.Count - 1]);
            if (temp < min)
            {
                indexLieu = 0;
                min = temp;
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
            int indexLieu = 1;
            int temp;
            int max = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[1]);
            for (int i = 1; i < T.ListeLieux.Count - 1; i++)
            {
                temp = FloydWarshall.Distance(T.ListeLieux[i], T.ListeLieux[i + 1]);
                if (temp > max)
                {
                    indexLieu = i;
                    max = temp;
                }
            }
            temp = FloydWarshall.Distance(T.ListeLieux[0], T.ListeLieux[T.ListeLieux.Count - 1]);
            if (temp > max)
            {
                indexLieu = 0;
                max = temp;
            }
            return indexLieu;
        }

        /// <summary>
        /// ajoute le premier élément d'une tournée à la fin de celle-ci (on économise trois secondes de réflexions, c'est bokou)
        /// </summary>
        /// <param name="tourne">liste à réorganiser</param>
        public static void FinCycle(Tournee tourne)
        {
            tourne.Add(tourne.ListeLieux[0]);
        }
    }
}
