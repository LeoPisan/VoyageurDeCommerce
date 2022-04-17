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
        private List<List<Lieu>> population; //population de solutions qui sera amenée à évoluer
        private List<Lieu> lieuxAgenerer;
        private int elitisme;
        private int nbGenerations; //nombre de générations à tester
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
            this.tauxMutation = tauxMutation;
            this.population = InitPop(taillePop);
            this.elitisme = elitisme;
            this.nbGenerations = nbGenerations;
        }

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            this.Tournee = new Tournee(listeLieux);

            this.InitPop(100); //création d'une population de départ de 100 individus (la taille changera par la suite en fonction des résultats des premiers tests
            for (int i = 0; i < this.nbGenerations; i++)
                this.Evoluer();

            this.MiseAjourFinale();
            //en cours de création
        }
        #endregion

        #region private methods
        //créée la population de départ
        private List<List<Lieu>> InitPop(int taillePop)
        {
            List<List<Lieu>> tempPop = new List<List<Lieu>>();
            for (int i = 0; i < taillePop; i++) //on génère des individus aléatoirement
            {
                this.lieuxAgenerer = this.Tournee.ListeLieux; //permet à LieuAleatoire() de ne générer chez un individus que des sommets n'ayant pas encore été ajoutés
                List<Lieu> tempIndividus = new List<Lieu>();
                for (int j = 0; j < this.Tournee.ListeLieux.Count; i++) //on génère les éléments d'un individus aléatoirement
                {
                    tempIndividus.Add(LieuAleatoire());
                }
                tempPop.Add(tempIndividus);
            }
            return tempPop;
        }

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
            List<List<Lieu>> tempPop = new List<List<Lieu>>();
            for (int i = 0; i < population.Count - 1;)
            {
                List<Lieu>[] couple = this.Selection(this.population);
                tempPop.Add(Crossover(couple[0], couple[1]));
            }
            this.population = tempPop;
        }

        //genere un enfant à partir de deux parents
        private List<Lieu> Crossover(List<Lieu> parent1, List<Lieu> parent2)
        {
            List<Lieu> retour = new List<Lieu>();
            var random = new Random();
            int indice = random.Next(this.Tournee.ListeLieux.Count - 1);
            for (int i = 0; i < indice; i++)
                retour[i] = parent1[i];
            if (retour.Count < this.Tournee.ListeLieux.Count - 1)
                for (int i = indice; i < this.Tournee.ListeLieux.Count - 1; i++)
                    retour[i] = parent2[i];
            return retour;
        }

        //retourne les n meilleurs individus de la population
        private List<List<Lieu>> MeilleursIndividus(int nbIndividus)
        {
            List<List<Lieu>> retour = new List<List<Lieu>>(this.population);
            while (retour.Count > nbIndividus)
            {
                List<Lieu> aRetirer = retour[0];
                int max = FloydWarshall.Distance(aRetirer[0], aRetirer[aRetirer.Count - 1]); //on initialise des valeurs de départ arbitraires, si on ne trouve pas plus grand ce seront elles qui seront retirées

                foreach (List<Lieu> l in retour) //on cherche l'individu avec la plus grande distance dans la population
                {
                    int distance = FloydWarshall.Distance(l[0], l[l.Count - 1]);
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
        private List<Lieu> MeilleurIndividu()
        {
            return MeilleursIndividus(1)[0];
        }

        //applique des mutations à une population selon le taux indiqué pour l'algorithme
        private List<List<Lieu>> Mutation(List<List<Lieu>> pop)
        {
            List<List<Lieu>> temp = new List<List<Lieu>>(pop);
            var random = new Random();
            for (int i = 0; i < temp.Count - 1; i++)
            {
                if (random.NextDouble() < this.tauxMutation)
                {
                    temp[i] = Outils.InverseElements(random.Next(temp[i].Count - 1), random.Next(temp[i].Count - 1), temp[i]);
                }
            }
            return temp;
        }

        //met à jour la tournée avec le meilleur individu de la population finale
        private void MiseAjourFinale()
        {
            this.Reset();
            foreach (Lieu l in MeilleurIndividu())
            {
                this.Tournee.Add(l);
                this.NotifyPropertyChanged("Tournee");
            }
        }
        #endregion

        #region protected methods
        /// <summary>
        /// calcule la fitness (l'efficacité) d'un individu
        /// </summary>
        /// <param name="individu">individu dont la fitness doit être calculée</param>
        /// <returns>fitness de l'individu</returns>
        protected double FitnessIndividu(List<Lieu> individu)
        {
            return 1 / (double)FloydWarshall.Distance(individu[0], individu[individu.Count - 1]);
        }

        /// <summary>
        /// effectue la sélection des individus passant d'une génération à une autre, varie selon les implémentations
        /// </summary>
        /// <returns>population suivante</returns>
        protected abstract List<Lieu>[] Selection(List<List<Lieu>> population);
        #endregion
    }
}
