using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections
{
    class Roulette
    {
        //private Object[,] roue;
        private Dictionary<Individu, double> roue;

        public Roulette(Population pop)
        {
            roue = new Dictionary<Individu, double>();
            foreach (Individu ind in pop.ListeIndividus)
            {
                roue.Add(ind, ind.Fitness);
            }
        }

        /// <summary>
        /// retourne un individu sélectionné selon sa fitness
        /// </summary>
        /// <returns>individu sélectionné</returns>
        public Individu LanceRoue()
        {
            Individu retour = null;

            double total = 0;
            foreach (KeyValuePair<Individu, double> ind in roue)
            {
                total += ind.Value;
            }

            double resultatCible = AlgoGenetique.Random.NextDouble() * total; //on prend un nombre flottant aléatoire inférieur à la somme des fitness de tous les éléments de la roue



            double cible = 0;
            foreach (KeyValuePair<Individu, double> ind in roue)
            {
                if (cible + ind.Value >= resultatCible)
                {
                    retour = ind.Key;
                    break;
                }
                else
                    cible += ind.Value;
            }

            return retour;
        }
        
        /// <summary>
        /// sélectionne un individu au hasard parmi la moitié la plus performante de la population
        /// </summary>
        /// <returns>individu sélectionné</returns>
        public Individu LanceRoueRang()
        {
            List<Individu> populationOrdonnee = (List<Individu>)this.roue.OrderByDescending(s => s.Value);
            List<Individu> tempParents = new List<Individu>();

            for (int i = 0; i <= populationOrdonnee.Count() / 2; i++)
                tempParents.Add(populationOrdonnee[i]);

            return tempParents[AlgoGenetique.Random.Next(tempParents.Count)];
        }

    }
}
