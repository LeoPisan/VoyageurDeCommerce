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
        private List<Lieu> aVisiter;

        public override string Nom => "Plus proche voisin";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            aVisiter = listeLieux;
            while (aVisiter.Count > 0)
            {
                Lieu suivant = MagasinProcheNonVisite();
                this.Tournee.ListeLieux.Add(suivant);
                aVisiter.Remove(suivant);
            }

        }

        private Lieu MagasinProcheNonVisite()
        {
            int min = FloydWarshall.Distance(dernierVisite, aVisiter[0]);
            Lieu retour = aVisiter[0];
            foreach (Lieu l in aVisiter)
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
