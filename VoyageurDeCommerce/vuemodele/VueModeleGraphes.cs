using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.parseur;

namespace VoyageurDeCommerce.vuemodele
{
    /// <summary>
    /// Vue modèle des graphes (Lieux et routes)
    /// </summary>
    public class VueModeleGraphes : Observable
    {
        ///<summary>Liste des fichiers pour la combobox</summary>
        private ObservableCollection<String> listeFichiers;
        public ObservableCollection<String> ListeFichiers => listeFichiers;
        ///<summary>Liste des lieux du graphe en cours</summary>
        private Dictionary<String,Lieu> listeLieux;
        public Dictionary<String, Lieu> ListeLieux => listeLieux;
        ///<summary>Liste des routes du graphe en cours</summary>
        private List<Route> listeRoutes;
        public List<Route> ListeRoutes => listeRoutes;
        /// <summary>Fichier sélectionné dans la combobox</summary>
        private String fichierSelectionne;
        private String fichierSelectionneMem;
        /// <summary>Le fichier doit-il être chargé ?</summary>
        private bool doitCharger;

        /// <summary>Fichier sélectionné pour le graphe </summary>
        public String FichierSelectionne {
            get => fichierSelectionne;
            set
            {
                if(fichierSelectionne != null) fichierSelectionneMem = fichierSelectionne;
                fichierSelectionne = value;
                this.ChargementFichier();
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>Constructeur par défaut</summary>
        public VueModeleGraphes()
        {
            //Initialisation
            this.listeLieux = new Dictionary<String, Lieu>();
            this.listeRoutes = new List<Route>();
            this.listeFichiers = new ObservableCollection<String>();
            this.fichierSelectionneMem = "";
            this.doitCharger = true;

            //Chargement de la liste des fichiers de graphes
            this.UpdateListeFichiers();

            //Creation d'un watcher 
            String chemin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\ressources\";
            FileSystemWatcher watcher = new FileSystemWatcher(chemin,"*.gph");
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
            watcher.EnableRaisingEvents = true;

        }

        /// <summary>Suppression d'un fichier-Graphe</summary>
        /// <param name="sender">Watcher</param>
        /// <param name="e">Le fichier supprimé</param>
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.doitCharger = false;
                this.UpdateListeFichiers();
                if (this.fichierSelectionneMem.Equals(e.Name))
                {
                    this.doitCharger = true;
                    this.FichierSelectionne = null;
                }
                else this.FichierSelectionne = this.fichierSelectionneMem;
                this.doitCharger = true;
            });
        }

        /// <summary>Renommage d'un fichier-Graphe</summary>
        /// <param name="sender">Watcher</param>
        /// <param name="e">Le fichier renommé</param>
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.doitCharger = false;
                if (e.OldName.Equals(this.fichierSelectionne))
                {
                    this.FichierSelectionne = e.Name;
                }
                this.UpdateListeFichiers();
                this.FichierSelectionne = this.fichierSelectionneMem;
                this.doitCharger = true;
            });
        }

        /// <summary>Création d'un nouveau fichier-Graphe</summary>
        /// <param name="sender">Watcher</param>
        /// <param name="e">Nouveau fichier</param>
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.doitCharger = false;
                this.UpdateListeFichiers();
                this.FichierSelectionne = this.fichierSelectionneMem;
                this.doitCharger = true;
            });
        }

        /// <summary>Mise-à-jour de la liste des fichiers</summary>
        private void UpdateListeFichiers()
        {
            //Récupération des noms de fichiers
            String chemin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\ressources\";
            String filtre = "*.gph";
            String[] fichiers = Directory.GetFiles(chemin, filtre);

            //Mise à jour de la liste
            this.listeFichiers.Clear();
            foreach(String fichier in fichiers)
            {
                String nomFichier = Path.GetFileName(fichier);
                this.listeFichiers.Add(nomFichier);
            }
        }

        /// <summary>Chargement du fichier</summary>
        private void ChargementFichier()
        {
            if(this.doitCharger)
            {
                this.ListeLieux.Clear();
                this.ListeRoutes.Clear();
                if (this.fichierSelectionne != null)
                {
                    //Parsage du fichier et mise-à-jour des listes de lieux
                    Parseur parseur = new Parseur(this.fichierSelectionne);
                    parseur.Parser();
                    this.listeLieux = parseur.ListeLieux;
                    this.listeRoutes = parseur.ListeRoutes;
                    this.NotifyPropertyChanged("Graphe");
                }
            }
        }
    }
}
