using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class AlgoPlusProcheVoisin : Algorithme
    {
        private Lieu dernierVisite;

        public override string Nom => "Plus proche voisin";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            
        }

        private Lieu MagasinProche()
        {
            int min = FloydWarshall.Distance(dernierVisite, this.Tournee.ListeLieux[0]);
            Lieu retour = this.Tournee.ListeLieux[0];
            foreach (Lieu l in this.Tournee.ListeLieux)
            {
                if (FloydWarshall.Distance(dernierVisite, l) <= min)
                {
                    retour = l;
                }
            }
            return retour;
        }
    }
}
