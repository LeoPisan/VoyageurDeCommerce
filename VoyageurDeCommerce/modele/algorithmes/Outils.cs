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

        public List<Lieu> InverseElements(int a, int b)
        {

        }
    }
}
