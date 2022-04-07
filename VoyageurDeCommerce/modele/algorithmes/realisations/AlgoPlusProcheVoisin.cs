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
    class AlgoPlusProcheVoisin : Algorithme
    {
        private Lieu dernierVisite;
        private List<Lieu> aVisiter;
        private Lieu usineDepart;

        public override string Nom => "Plus proche voisin";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            usineDepart = dernierVisite = Outils.UsineDepart(listeLieux);
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            aVisiter = listeLieux;
            while (aVisiter.Count > 0)
            {
                Lieu suivant = MagasinProcheNonVisite();
                this.Tournee.Add(suivant);
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
                aVisiter.Remove(suivant);
                dernierVisite = suivant;
            }
            this.Tournee.Add(usineDepart);
            this.NotifyPropertyChanged("Tournee");
            stopwatch.Stop();
        }

        /// <summary>
        /// renvoie le magasin non visité le plus proche
        /// </summary>
        /// <returns></returns>
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
