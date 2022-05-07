using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections
{
    class Roulette
    {
        #region attributes
        private Dictionary<Individu, double> roue;
        private List<Individu> listeRoue;
        #endregion

        public Roulette(Population pop)
        {
            roue = new Dictionary<Individu, double>();
            listeRoue = new List<Individu>();

            foreach (Individu ind in pop.ListeIndividus)
            {
                roue.Add(ind, ind.Fitness);
                listeRoue.Add(ind);
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
            var tempParents = listeRoue.OrderByDescending(s => s.Fitness);
            List<Individu> listCandidats = new List<Individu>();

            foreach (Individu i in tempParents)
                listCandidats.Add(i);

            int fin = listCandidats.Count / 2;
            for (int i = 0; i < fin; i++)
                listCandidats.RemoveAt(listCandidats.Count - 1 - i);

            return listCandidats[AlgoGenetique.Random.Next(listCandidats.Count)];
        }

    }
}
