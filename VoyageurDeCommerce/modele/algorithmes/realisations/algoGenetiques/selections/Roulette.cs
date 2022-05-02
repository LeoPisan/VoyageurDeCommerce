using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections
{
    class Roulette
    {
        private Object[,] roue;
        public Roulette(Population pop)
        {
            roue = new Object[pop.Size, 2]; //assez peu solide, chercher un moyen de contraindre les types double et individu
            roue[0, 0] = pop.ListeIndividus[0];
            roue[0, 1] = pop.ListeIndividus[0].Fitness;
            for (int i = 1; i <= pop.Size;)
            {

                roue[i, 0] = pop.ListeIndividus[i];
                roue[i, 1] = pop.ListeIndividus[i].Fitness + (double)roue[i - 1, 1];
            }
        }

        public Individu LanceRoue()
        {
            Individu retour = null;
            var random = new Random();
            double fitness = (double)random.Next((int)(100 * (double)roue[roue.GetLongLength(0), 1]))/100;
            for (int i = 0; i < roue.GetLongLength(0);)
            {
                if ((i > 0) && (fitness > (double)roue[i - 1, 1]) && (fitness <= (double)roue[i, 1]))
                {
                    retour = (Individu)roue[i, 0];
                    break;
                }
                else if ((i == 0) && ((double)roue[0, 1]) >= fitness)
                {
                    retour = (Individu)roue[0, 0];
                    break;
                }
            }
            return retour;
        }
    }
}
