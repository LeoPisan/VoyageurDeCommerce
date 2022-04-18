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
        /// <summary>
        /// efficacité de l'individu, indique à quel point il est optimal
        /// </summary>
        public double Fitness => 1 / (double)FloydWarshall.Distance(this.ListeLieux[0], this.ListeLieux[this.ListeLieux.Count - 1]);
        public int Size => this.ListeLieux.Count - 1;
        #endregion

        /// <summary>
        /// création d'un individu aléatoire respectant des lieux de passages obligatoires
        /// </summary>
        /// <param name="lieuxAgenerer">lieux de passages obligatoires de l'individu</param>
        public Individu(List<Lieu> lieuxAgenerer)
        {
            List<Lieu> aGenerer = new List<Lieu>(lieuxAgenerer);
            for (int i = 0; i < lieuxAgenerer.Count;)
            {
                this.Add(LieuAleatoire(ref aGenerer));
            }
        }

        /// <summary>
        /// création d'un individu par crossover (reproduction)
        /// </summary>
        /// <param name="parent1">premier parent utilisé</param>
        /// <param name="parent2">deuxième parent utilisé</param>
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
