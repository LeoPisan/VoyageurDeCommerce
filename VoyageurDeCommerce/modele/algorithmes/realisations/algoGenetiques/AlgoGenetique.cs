using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;
using System.Diagnostics;

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
        private int nbGenerations; //nombre de générations à tester
        private int taillePop;
        private double tauxMutation;
        private static Random random = new Random();
        #endregion

        public static Random Random { get => random; }
        protected Population Population { get => population; }

        #region public methods
        /// <summary>
        /// créée un algorithme génétique de base
        /// </summary>
        /// <param name="taillePop">taille de la population initiale</param>
        /// <param name="elitisme">performance visée pour le résultat</param>
        public AlgoGenetique(int taillePop, int nbGenerations, double tauxMutation)
        {
            this.taillePop = taillePop;
            this.tauxMutation = tauxMutation;
            this.nbGenerations = nbGenerations;
        }

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.lieuxAgenerer = new List<Lieu>(listeLieux);
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            this.Tournee = new Tournee(listeLieux);
            this.population = new Population(taillePop, this.Tournee);

            /*
            for (int i = 0; i < this.nbGenerations; i++)
                this.Evoluer();
            */

            bool fin = false;
            int generations = this.nbGenerations;

            while (!fin)
            {
                Population tempPop = new Population(this.population);
                this.Evoluer();
                if (this.MeilleurIndividu().Fitness <= tempPop.MeilleurIndividu().Fitness)
                {
                    this.population = new Population(tempPop);
                    generations--;
                }
                else
                    generations = this.nbGenerations;

                if (generations <= 0)
                {
                    fin = true;
                }
            }
            stopwatch.Stop();
            this.MiseAjourFinale();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// cherche le meilleur individu de la population
        /// </summary>
        /// <returns>meilleur individu</returns>
        public Individu MeilleurIndividu()
        {
            return this.population.MeilleurIndividu();
        }
        #endregion

        #region private methods
        //fait évoluer la population de rang n à la population de rang n + 1
        private void Evoluer()
        {
            Population tempPop = new Population();
            for (int i = 0; i <= population.Size; i++)
            {
                Individu[] couple = this.Selection(this.population);
                tempPop.Add(new Individu(couple[0], couple[1]));
            }
            tempPop.Muter(tauxMutation);
            this.population = tempPop;
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

        /// <summary>
        /// effectue la sélection des individus passant d'une génération à une autre, varie selon les implémentations
        /// </summary>
        /// <returns>population suivante</returns>
        protected abstract Individu[] Selection(Population population);
    }
}
