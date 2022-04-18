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
        public Population()
        {
            listeIndividus = new List<Individu>();
        }

        public Population(List<Individu> listeIndividus)
        {
            ListeIndividus = listeIndividus;
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
            for (int i = 0; i < taillePop; i++) //on génère des individus aléatoirement
            {
                this.Add(new Individu(tourneeModele.ListeLieux));
            }
        }

        public void Add(Individu i)
        {
            this.ListeIndividus.Add(i);
        }

        public void Remove(Individu i)
        {
            this.ListeIndividus.Remove(i);
        }

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
