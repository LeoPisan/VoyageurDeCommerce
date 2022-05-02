using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques
{
    class Population
    {
        private List<Individu> listeIndividus;

        #region properties
        public List<Individu> ListeIndividus { get => listeIndividus; set => listeIndividus = value; }
        public int Size => ListeIndividus.Count - 1;
        #endregion

        #region methods
        /// <summary>
        /// création d'une population vierge
        /// </summary>
        public Population()
        {
            listeIndividus = new List<Individu>();
        }

        /// <summary>
        /// construction d'une population reprenant une liste d'individus de départ
        /// </summary>
        /// <param name="listeIndividus">liste d'individus à reprendre dans la nouvelle population</param>
        public Population(List<Individu> listeIndividus)
        {
            ListeIndividus = new List<Individu>(listeIndividus);
        }

        /// <summary>
        /// créée une population par copie
        /// </summary>
        /// <param name="p">population à recopier</param>
        public Population(Population p)
        {
            this.listeIndividus = p.listeIndividus;
        }
        
        /// <summary>
        /// initialise une population aléatoire
        /// </summary>
        /// <param name="taillePop">taille de la population à générer</param>
        /// <param name="tourneeModele">tournée contenant les lieux à visiter</param>
        public Population(int taillePop, Tournee tourneeModele)
        {
            this.listeIndividus = new List<Individu>();
            for (int i = 0; i < taillePop; i++) //on génère des individus aléatoirement
            {
                this.Add(new Individu(tourneeModele.ListeLieux));
            }
        }

        public void Add(Individu ind)
        {
            this.ListeIndividus.Add(ind);
        }

        public void Remove(Individu i)
        {
            this.ListeIndividus.Remove(i);
        }

        /// <summary>
        /// fgait muter une population selon un taux de mutation défini à l'avance
        /// </summary>
        /// <param name="tauxMutation">taux de mutation à appliquer</param>
        public void Muter(double tauxMutation)
        {
            var random = new Random();
            foreach (Individu i in listeIndividus)
            {
                if (random.NextDouble() < tauxMutation)
                    i.Muter();
            }
        }
        #endregion
    }
}
