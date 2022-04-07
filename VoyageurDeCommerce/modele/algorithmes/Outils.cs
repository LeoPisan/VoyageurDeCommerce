using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;

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
            List<Lieu> liste = listeEntree;
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
    }
}
