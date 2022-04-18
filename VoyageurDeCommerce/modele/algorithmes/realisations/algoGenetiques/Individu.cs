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
        #region properties
        public double Fitness => 1 / (double)FloydWarshall.Distance(this.ListeLieux[0], this.ListeLieux[this.ListeLieux.Count - 1]);
        public int Size => this.ListeLieux.Count - 1;
        #endregion

        public Individu(List<Lieu> lieuxAgenerer)
        {
            List<Lieu> aGenerer = new List<Lieu>(lieuxAgenerer);
            for (int i = 0; i < lieuxAgenerer.Count;)
            {
                this.Add(LieuAleatoire(ref aGenerer));
            }
        }

        public Individu(Individu parent1, Individu parent2)
        {
            this.ListeLieux = new List<Lieu>();
            var random = new Random();
            int indice = random.Next(parent1.Size);
            for (int i = 0; i < indice; i++)
                this.ListeLieux[i] = parent1.ListeLieux[i];
            if (this.ListeLieux.Count < parent1.Size + 1)
                for (int i = indice; i < parent1.Size; i++)
                    this.ListeLieux[i] = parent2.ListeLieux[i];
        }

        /// <summary>
        /// effectue une mutation aléatoire sur l'individu en inversant deux de ses éléments
        /// </summary>
        public void Muter() 
        {
            var random = new Random();
            this.ListeLieux = Outils.InverseElements(random.Next(this.ListeLieux.Count - 1), random.Next(this.ListeLieux.Count - 1), this.ListeLieux);
        }

        private Lieu LieuAleatoire(ref List<Lieu> lieuxAgenerer)
        {
            var random = new Random();
            int indice = random.Next(lieuxAgenerer.Count - 1);
            Lieu retour = lieuxAgenerer[indice];
            lieuxAgenerer.RemoveAt(indice);
            return (retour);
        }
    }
}
