using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VoyageurDeCommerce.exception.realisations;
using VoyageurDeCommerce.modele.algorithmes;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.vuemodele
{
    /// <summary> Vue-Modèle pour la liste des algorithmes </summary>
    public class VueModeleAlgorithmes : Observable
    {
        /// <summary> Liste des algorithmes</summary>
        private readonly ObservableCollection<Algorithme> listeDesAlgorithmes;
        public ObservableCollection<Algorithme> ListeDesAlgorithmes => listeDesAlgorithmes;
        /// <summary> Algorithme sélectionné dans la ComboBox </summary>
        private Algorithme algorithmeSelectionne;
        public Algorithme AlgorithmeSelectionne
        {
            get => algorithmeSelectionne;
            set
            {
                algorithmeSelectionne = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>Constructeur par défaut</summary>
        public VueModeleAlgorithmes()
        {
            this.algorithmeSelectionne = null;
            this.listeDesAlgorithmes = new ObservableCollection<Algorithme>();
            foreach (TypeAlgorithme type in (TypeAlgorithme[])Enum.GetValues(typeof(TypeAlgorithme)))
            {
                this.listeDesAlgorithmes.Add(FabriqueAlgorithme.Creer(type));
            }
        }

        /// <summary>
        /// Execute l'algorithme sélectionné
        /// </summary>
        /// <param name="listeLieux">Liste des lieux</param>
        /// <param name="listeRoutes">Liste des routes</param>
        public void ExecuterAlgorithmeSelectionne(List<Lieu> listeLieux, List<Route> listeRoutes)
        {
            if (this.algorithmeSelectionne == null) throw new ExceptionAlgorithme("Aucun algorithme sélectionné !");
            this.algorithmeSelectionne.Reset();
            this.algorithmeSelectionne.Executer(listeLieux, listeRoutes);
        }

    }
}
