using System.Collections.Generic;
using System.ComponentModel;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.vuemodele;

namespace VoyageurDeCommerce.modele.algorithmes
{
    public abstract class Algorithme : Observable
    {
        /// <summary> Nom de l'algorithme </summary>
        public abstract string Nom { get; }
        private Tournee tournee;
        public Tournee Tournee
        {
            get => tournee;
            protected set => tournee = value;
        }

        // <summary>Temps d'execution de l'algorithme</summary>
        private long tempsExecution;
        public long TempsExecution
        {
            get => tempsExecution;
            protected set
            {
                tempsExecution = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Algorithme()
        {
            this.tournee = new Tournee();
        }

        /// <summary>
        /// Reset de la tournée lors d'un changement de graphe
        /// </summary>
        public void Reset()
        {
            this.tournee = new Tournee();
        }


        /// <summary> Exécution de l'algorithme </summary>
        /// <param name="tour">Tour modifié par l'algorithme (ne pas travailler sur une copie)</param>
        /// <param name="listeLieux">Liste des lieux à considérer</param>
        public abstract void Executer(List<Lieu> listeLieux, List<Route> listeRoute);
    }
}
