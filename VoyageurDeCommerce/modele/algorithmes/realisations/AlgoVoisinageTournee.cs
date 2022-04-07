using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class AlgoVoisinageTournee : Algorithme
    {
        public override string Nom => "Voisinage d'une tournée";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            this.Tournee.ListeLieux = listeLieux;
            bool fin = false;
            while (!fin)
            {
                foreach (Lieu l in Tournee.ListeLieux)
                {

                }
            }

            stopwatch.Stop();
        }
    }
}
