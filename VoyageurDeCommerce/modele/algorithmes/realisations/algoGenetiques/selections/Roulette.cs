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
            /*
            roue = new Object[pop.Size, 2]; //assez peu solide, chercher un moyen de contraindre les types double et individu
            roue[0, 0] = pop.ListeIndividus[0];
            roue[0, 1] = pop.ListeIndividus[0].Fitness;
            for (int i = 1; i < pop.Size; i++)
            {

                roue[i, 0] = pop.ListeIndividus[i];
                roue[i, 1] = pop.ListeIndividus[i].Fitness + (double)roue[i - 1, 1];
            }
            */
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

            double resultatCible = AlgoGenetique.random.NextDouble() * total; //on prend un nombre flottant aléatoire inférieur à la somme des fitness de tous les éléments de la roue



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
    }
}
