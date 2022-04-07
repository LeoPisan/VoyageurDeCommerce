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
        public static Lieu UsineDepart(List<Lieu> liste)
        {
            Lieu retour = liste[0];
            foreach (Lieu l in liste)
            {
                if (l.Type == TypeLieu.USINE)
                    retour = l;
            }
            return retour;
        }
    }
}
