using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques
{
    class Individu : Tournee
    {
        public double Fitness => 1 / (double)FloydWarshall.Distance(this.ListeLieux[0], this.ListeLieux[this.ListeLieux.Count - 1]);

        /// <summary>
        /// effectue une mutation aléatoire sur l'individu en inversant deux de ses éléments
        /// </summary>
        public void Muter() 
        {
            var random = new Random();
            this.ListeLieux = Outils.InverseElements(random.Next(this.ListeLieux.Count - 1), random.Next(this.ListeLieux.Count - 1), this.ListeLieux);
        }
    }
}
