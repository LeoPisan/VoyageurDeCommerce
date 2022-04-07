using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.algorithmes;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.vuemodele
{
    /// <summary> Vue-Modèle de l'historique des tours</summary>
    public class VueModeleHistorique : Observable
    {
        /// <summary>Algorithme observé par le vue-modèle</summary>
        private Algorithme algorithme;
        public Algorithme Algorithme
        {
            set
            {
                if (this.algorithme != null) this.algorithme.PropertyChanged -= Algorithme_PropertyChanged;
                this.algorithme = value;
                this.Historique.Clear();
                if (algorithme != null) this.algorithme.PropertyChanged += Algorithme_PropertyChanged;
            }
        }
        /// <summary>Historique des tournées sauvées par l'algo</summary>
        private List<Tournee> historique;
        public List<Tournee> Historique => historique;

        /// <summary>Tournée sélectionnée pour l'affichage</summary>
        private Tournee tourneeSelectionnee;
        public Tournee TourneeSelectionee
        {
            get => tourneeSelectionnee;
            set
            {
                tourneeSelectionnee = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged("TextTourneeSelectionnee");
            }
        }

        /// <summary>Texte d'affichage de la tournée</summary>
        public String TextTourneeSelectionnee
        {
            get
            {
                String res = "";
                if (tourneeSelectionnee != null) res = tourneeSelectionnee.ToString();
                return res;
            }
        }


        /// <summary>Temps d'execution de l'algorithme</summary>
        public long TempsExecution => this.algorithme.TempsExecution;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public VueModeleHistorique()
        {
            this.algorithme = null;
            this.historique = new List<Tournee>();
        }


        /// <summary>
        /// Modification d'une propriété de l'algorithme
        /// </summary>
        /// <param name="sender">L'algorithme</param>
        /// <param name="e">La propriété</param>
        private void Algorithme_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Tournee")
            {
                historique.Add(new Tournee(this.algorithme.Tournee));
                this.NotifyPropertyChanged("Historique");
            }
            if (e.PropertyName == "TempsExecution")
            {
                this.NotifyPropertyChanged("TempsExecution");
            }
        }
        /// <summary>Clear l'historique</summary>
        public void Clear()
        {
            this.Historique.Clear();
            this.TourneeSelectionee = null;
        }
    }
}
