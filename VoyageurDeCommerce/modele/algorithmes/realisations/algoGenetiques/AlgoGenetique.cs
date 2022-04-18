using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques
{
    /// <summary>
    /// classe contenant le coeur des algorithmes génétiques, ses dérivés implémenteront différentes méthodes de sélection
    /// </summary>
    abstract class AlgoGenetique : Algorithme
    {
        #region attributes
        private Population population; //population de solutions qui sera amenée à évoluer
        private List<Lieu> lieuxAgenerer;
        private int elitisme;
        private int nbGenerations; //nombre de générations à tester
        private int taillePop;
        private double tauxMutation;
        #endregion

        #region public methods
        /// <summary>
        /// créée un algorithme génétique de base
        /// </summary>
        /// <param name="taillePop">taille de la population initiale</param>
        /// <param name="elitisme">performance visée pour le résultat</param>
        public AlgoGenetique(int taillePop, int elitisme, int nbGenerations, double tauxMutation)
        {
            this.taillePop = taillePop;
            this.tauxMutation = tauxMutation;
            this.elitisme = elitisme;
            this.nbGenerations = nbGenerations;
        }

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {

            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            this.Tournee = new Tournee(listeLieux);
            this.population = new Population(taillePop, this.Tournee);


            this.InitPop(100); //création d'une population de départ de 100 individus (la taille changera par la suite en fonction des résultats des premiers tests
            for (int i = 0; i < this.nbGenerations; i++)
                this.Evoluer();

            this.MiseAjourFinale();
            //en cours de création
        }
        #endregion

        #region private methods
        //génère un lieu aléatoire compris dans la tournée et n'ayant pas encore été inséré
        private Lieu LieuAleatoire()
        {
            var random = new Random();
            int indice = random.Next(this.lieuxAgenerer.Count - 1);
            Lieu retour = this.lieuxAgenerer[indice];
            this.lieuxAgenerer.RemoveAt(indice);
            return (retour);
        }

        //fait évoluer la population de rang n à la population de rang n + 1
        private void Evoluer()
        {
            Population tempPop = new Population();
            for (int i = 0; i < population.Size;)
            {
                Individu[] couple = this.Selection(this.population);
                tempPop.Add(Crossover(couple[0], couple[1]));
            }
            this.population = tempPop;
        }

        //genere un enfant à partir de deux parents
        private Individu Crossover(Individu parent1, Individu parent2)
        {
            Individu retour = new Individu();
            var random = new Random();
            int indice = random.Next(this.Tournee.ListeLieux.Count - 1);
            for (int i = 0; i < indice; i++)
                retour.ListeLieux[i] = parent1.ListeLieux[i];
            if (retour.ListeLieux.Count < this.Tournee.ListeLieux.Count - 1)
                for (int i = indice; i < this.Tournee.ListeLieux.Count - 1; i++)
                    retour.ListeLieux[i] = parent2.ListeLieux[i];
            return retour;
        }

        //retourne les n meilleurs individus de la population
        private Population MeilleursIndividus(int nbIndividus)
        {
            Population retour = new Population(this.population);
            while (retour.Size > nbIndividus)
            {
                Individu aRetirer = retour.ListeIndividus[0];
                int max = FloydWarshall.Distance(aRetirer.ListeLieux[0], aRetirer.ListeLieux[aRetirer.ListeLieux.Count - 1]); //on initialise des valeurs de départ arbitraires, si on ne trouve pas plus grand ce seront elles qui seront retirées

                foreach (Individu l in retour.ListeIndividus) //on cherche l'individu avec la plus grande distance dans la population
                {
                    int distance = FloydWarshall.Distance(l.ListeLieux[0], l.ListeLieux[l.ListeLieux.Count - 1]);
                    if (distance > max)
                    {
                        max = distance;
                        aRetirer = l;
                    }
                }
                retour.Remove(aRetirer); //on retire cet individu et on répète jusqu'à avoir une liste de la taille demandée
            }
            return retour;
        }

        //retourne le meilleur individu de la population
        private Individu MeilleurIndividu()
        {
            return MeilleursIndividus(1).ListeIndividus[0];
        }

        //applique des mutations à une population selon le taux indiqué pour l'algorithme
        private List<Individu> Mutation(List<Individu> pop)
        {
            List<Individu> temp = new List<Individu>(pop);
            var random = new Random();
            for (int i = 0; i < temp.Count - 1; i++)
            {
                if (random.NextDouble() < this.tauxMutation)
                {
                    temp[i].Muter();
                }
            }
            return temp;
        }

        //met à jour la tournée avec le meilleur individu de la population finale
        private void MiseAjourFinale()
        {
            this.Reset();
            foreach (Lieu l in MeilleurIndividu().ListeLieux)
            {
                this.Tournee.Add(l);
                this.NotifyPropertyChanged("Tournee");
            }
        }
        #endregion

        #region protected methods
        /// <summary>
        /// effectue la sélection des individus passant d'une génération à une autre, varie selon les implémentations
        /// </summary>
        /// <returns>population suivante</returns>
        protected abstract Individu[] Selection(Population population);
        #endregion
    }
}
