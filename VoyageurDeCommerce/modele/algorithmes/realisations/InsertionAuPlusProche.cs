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
    class InsertionAuPlusProche : Algorithme
    {
        public override string Nom => "Insertion au plus proche";

        public override void Executer(List<Lieu> lieux, List<Route> routes)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FloydWarshall.calculerDistances(lieux, routes);
            foreach (Lieu lieu in lieux)
            {
                this.Tournee.Add(lieu);
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
        }
    }
}
