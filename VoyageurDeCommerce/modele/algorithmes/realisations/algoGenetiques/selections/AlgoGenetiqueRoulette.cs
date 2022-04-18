using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections
{
    class AlgoGenetiqueRoulette : AlgoGenetique
    {
        public override string Nom => "Algorithme génétique à sélection par roulette";

        public AlgoGenetiqueRoulette() : base(100,100,0) { }

        /// <summary>
        /// retourne un couple d'individus sélectionnés selon la méthode de la sélection par roulette
        /// </summary>
        /// <param name="population">population dans laquelle effectuer la sélection</param>
        /// <returns>couple sélectionné</returns>
        protected override Individu[] Selection(Population population)
        {
            Individu[] retour = new Individu[2];
            for (int i = 0; i < 2;)
            {
                retour[i] = this.SelectionUnique(population);
            }
            return retour;
        }

        private Individu SelectionUnique(Population population)
        {
            Roulette roue = new Roulette(population);
            return roue.LanceRoue();
        }
    }
}
