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
        public double Fitness => 1 / (double)FloydWarshall.Distance(this.ListeLieux[0], this.ListeLieux[this.Size]);
        /// <summary>
        /// utilisée pour parcourir l'individu
        /// </summary>
        public int Size => this.ListeLieux.Count - 1;
        #endregion

        /// <summary>
        /// création d'un individu aléatoire respectant des lieux de passages obligatoires
        /// </summary>
        /// <param name="lieuxAgenerer">lieux de passages obligatoires de l'individu</param>
        public Individu(List<Lieu> lieuxAgenerer)
        {
            List<Lieu> aGenerer = new List<Lieu>(lieuxAgenerer);
            for (int i = 0; i < lieuxAgenerer.Count; i++)
            {
                Lieu l = lieuxAgenerer[i];
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
                this.ListeLieux.Add(parent1.ListeLieux[i]);

            if (this.ListeLieux.Count < parent1.Size + 1)
                for (int i = indice; i <= parent1.Size; i++)
                {
                    foreach(Lieu lieu in parent2.ListeLieux)
                    {
                        if (!this.ListeLieux.Contains(lieu))
                            this.Add(lieu);
                    }
                }
        }

        /// <summary>
        /// effectue une mutation aléatoire sur l'individu en inversant deux de ses éléments
        /// </summary>
        public void Muter() 
        {
            this.ListeLieux = Outils.InverseElements(AlgoGenetique.Random.Next(this.ListeLieux.Count - 1), AlgoGenetique.Random.Next(this.ListeLieux.Count - 1), this.ListeLieux);
        }

        //renvoie un lieu généré aléatoirement dans une liste de lieux à utiliser
        private Lieu LieuAleatoire(ref List<Lieu> lieuxAgenerer)
        {
            int indice = AlgoGenetique.Random.Next(lieuxAgenerer.Count);
            Lieu retour = lieuxAgenerer[indice];
            lieuxAgenerer.RemoveAt(indice);
            return (retour);
        }
    }
}
