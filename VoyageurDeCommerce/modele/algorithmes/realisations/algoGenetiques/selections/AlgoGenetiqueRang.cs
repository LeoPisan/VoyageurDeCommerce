using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques
{
    class AlgoGenetiqueRang : AlgoGenetique
    {
        public override string Nom => "Algorithme génétique à sélection par rang";

        public AlgoGenetiqueRang(): base(1000, 200, 0) { } //valeurs de test à changer après la complétion de l'implémentation
        
        protected override Individu[] Selection(Population population)
        {
            Individu[] retour = new Individu[2];

            for (int i = 0; i < 2; i++)
                retour[i] = this.SelectionUnique(population);

            return retour;
        }
        
        
        private Individu SelectionUnique(Population population)
        {
            Roulette roue = new Roulette(population);
            return roue.LanceRoueRang();
        }
        
        
    }
}
