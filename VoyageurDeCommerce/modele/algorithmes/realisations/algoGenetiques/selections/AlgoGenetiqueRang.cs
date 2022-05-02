using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques
{
    class AlgoGenetiqueRang : AlgoGenetique
    {
        public override string Nom => "Algorithme génétique à sélection par rang";

        public AlgoGenetiqueRang(): base(100, 100, 0) { } //valeurs de test à changer après la complétion de l'implémentation

        protected override Individu[] Selection(Population population)
        {
            Individu[] retour = new Individu[2];



            return retour;
        }

        /*
        private Individu SelectionUnique(Population population)
        {
            Individu retour;
            Population tempPop = new Population(population);


        }
        */
    }
}
