using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VoyageurDeCommerce.exception;
using VoyageurDeCommerce.exception.realisations;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.vue.composants;
using VoyageurDeCommerce.vue.ressources;
using VoyageurDeCommerce.vuemodele;

namespace VoyageurDeCommerce.vue
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class FenetrePrincipale : Window
    {
        private readonly VueModeleAlgorithmes vmAlgorithme;
        private readonly VueModeleGraphes vmGraphes;
        private readonly VueModeleHistorique vmHistorique;

        private readonly ScaleTransform layoutTransform;
        private readonly Dictionary<Route, Line> lignes;

        private readonly List<PanelTournee> listePanelTournee;

        private AnimationColoration animationColoration;

        public FenetrePrincipale()
        {
            InitializeComponent();
            try
            {
                this.vmAlgorithme = new VueModeleAlgorithmes();
                this.vmGraphes = new VueModeleGraphes();
                this.vmHistorique = new VueModeleHistorique();
                this.listePanelTournee = new List<PanelTournee>();

                this.ComboBoxAlgorithme.DataContext = this.vmAlgorithme;
                this.vmAlgorithme.PropertyChanged += VmAlgorithme_PropertyChanged;
                this.ComboBoxFichier.DataContext = this.vmGraphes;
                this.vmGraphes.PropertyChanged += VmGraphes_PropertyChanged;
                this.vmHistorique.PropertyChanged += VmHistorique_PropertyChanged;
                this.TextTournee.DataContext = this.vmHistorique;

                this.layoutTransform = new ScaleTransform();
                this.ScrollContent.LayoutTransform = this.layoutTransform;

                this.lignes = new Dictionary<Route, Line>();
                this.Icon = ImagesManager.GetImage("ICON");
            }
            catch (ExceptionVdC e)
            {
                ExceptionBox.Show(e);
            }

        }

        /// <summary>
        /// Reset l'ihm entre deux algo
        /// </summary>
        private void ResetIHM()
        {
            this.StackStory.Children.Clear();
            this.listePanelTournee.Clear();
            this.TextBoxDistance.Text = "Distance";
            this.vmHistorique.Clear();
        }

        /// <summary>Méthode appelée si une property du vueModele Historique change</summary>
        /// <param name="sender">vmHistorique</param>
        /// <param name="e">Property ayant changée</param>
        private void VmHistorique_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Ajout d'une tournée dans l'historique
            if (e.PropertyName.Equals("Historique"))
            {
                Tournee tournee = vmHistorique.Historique[vmHistorique.Historique.Count - 1];
                PanelTournee panel = new PanelTournee(tournee, vmHistorique.Historique.Count - 1);
                panel.MouseDown += PanelTournee_MouseDown;
                this.StackStory.Children.Add(panel);
                this.listePanelTournee.Add(panel);
                this.TextBoxDistance.Text = tournee.Distance.ToString();
            }
            else if (e.PropertyName.Equals("TourneeSelectionee"))
            {
                if (this.vmHistorique.TourneeSelectionee != null)
                {
                    this.ColorierTournee(this.vmHistorique.TourneeSelectionee);
                }
            }
            else if (e.PropertyName.Equals("TempsExecution"))
            {
                MessageBox.Show(vmHistorique.TempsExecution.ToString() + " ms", "Temps d'exécution de l'algorithme", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>Click sur un panel de tournée</summary>
        /// <param name="sender">Le panel de tournée</param>
        /// <param name="e">Le click</param>
        private void PanelTournee_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (PanelTournee panel in this.listePanelTournee)
            {
                panel.Deselectionner();
            }
           ((PanelTournee)sender).Selectionner();
            this.vmHistorique.TourneeSelectionee = ((PanelTournee)sender).Tournee;

        }

        private void ColorierTournee(Tournee tournee)
        {
            if (this.animationColoration != null) this.animationColoration.Stop();
            this.animationColoration = new AnimationColoration(tournee, this);
            this.animationColoration.Start();
        }

        public void ColorierRoutes(List<Route> routes)
        {
            this.CleanColoration();
            foreach (Route route in routes)
            {
                this.lignes[route].Stroke = Brushes.Red;
            }
        }

        public void CleanColoration()
        {
            foreach (Line line in this.lignes.Values)
            {
                line.Stroke = Brushes.Black;
            }
        }

        /// <summary>Méthode appelée si une property du vueModele Algorithme change</summary>
        /// <param name="sender">vmAlgorithme</param>
        /// <param name="e">Property ayant changée</param>
        private void VmAlgorithme_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Changement de l'algorithme selectionné
            if (e.PropertyName.Equals("AlgorithmeSelectionne"))
            {
                this.vmHistorique.Algorithme = this.vmAlgorithme.AlgorithmeSelectionne;
            }
            //Changement du temps 
        }

        /// <summary>Méthode appelée si une property du vueModele Graphes change</summary>
        /// <param name="sender">vmGraphe</param>
        /// <param name="e">Property ayant changée</param>
        private void VmGraphes_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Dessin du graphe
            if (e.PropertyName.Equals("Graphe"))
            {
                //On efface le contenu du canvas et de l'history
                this.ResetIHM();
                this.CanvasGraphe.Children.Clear();
                //On dessine les lieux
                int maxX = 0;
                int maxY = 0;
                int minX = Int32.MaxValue;
                int minY = Int32.MaxValue;
                foreach (Lieu lieu in this.vmGraphes.ListeLieux.Values)
                {
                    minX = Math.Min(minX, lieu.X);
                    minY = Math.Min(minY, lieu.Y);
                }
                foreach (Lieu lieu in this.vmGraphes.ListeLieux.Values)
                {
                    ImageLieu image = new ImageLieu(lieu, minX, minY);
                    maxX = Math.Max(DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (lieu.X - minX) + DisplaySettings.TailleLieu, maxX);
                    maxY = Math.Max(DisplaySettings.TailleLieu * DisplaySettings.FacteurEspacement * (lieu.Y - minY) + DisplaySettings.TailleLieu, maxY);
                    this.CanvasGraphe.Children.Add(image);
                }
                this.CanvasGraphe.Width = maxX;
                this.CanvasGraphe.Height = maxY;
                /*this.ScrollContent.Width = Math.Max(this.ScrollViewerGraphe.Width, this.CanvasGraphe.Width);
                this.ScrollContent.Height = Math.Max(this.ScrollViewerGraphe.Height, this.CanvasGraphe.Height);*/
                if (maxX > 0 && maxY > 0)
                {
                    this.layoutTransform.ScaleX = Math.Min(1100.0 / maxX, 600.0 / maxY);
                    this.layoutTransform.ScaleY = Math.Min(1100.0 / maxX, 600.0 / maxY);
                }

                //On dessine les routes
                foreach (Route route in this.vmGraphes.ListeRoutes)
                {
                    Line ligne = FabriqueLine.Creer(route, minX, minY);
                    this.CanvasGraphe.Children.Add(ligne);
                    TextBlock txtDistance = new TextBlock
                    {
                        Text = route.Distance.ToString(),
                        Background = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5)
                    };
                    Canvas.SetLeft(txtDistance, (ligne.X1 + ligne.X2 - 10.78 * txtDistance.Text.Length - 10) / 2.0);
                    Canvas.SetTop(txtDistance, (ligne.Y1 + ligne.Y2 - 36.6) / 2.0);
                    this.CanvasGraphe.Children.Add(txtDistance);
                    this.lignes[route] = ligne;
                }

            }
        }

        /// <summary> Click sur le bouton Execution : Exécute l'algorithme sélectionné</summary>
        /// <param name="sender">Le bouton</param>
        /// <param name="e">Parametres du click</param>
        private void BoutonExecution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ResetIHM();
                if (this.vmAlgorithme.AlgorithmeSelectionne == null) throw new ExceptionAlgorithme("Aucun algorithme sélectionné !");
                this.vmAlgorithme.ExecuterAlgorithmeSelectionne(new List<Lieu>(vmGraphes.ListeLieux.Values), new List<Route>(vmGraphes.ListeRoutes));
            }
            catch (ExceptionAlgorithme exception)
            {
                ExceptionBox.Show(exception);
            }
        }

        private void OnMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
            {
                this.layoutTransform.ScaleX += 0.1;
                this.layoutTransform.ScaleY += 0.1;
            }
            else
            {
                this.layoutTransform.ScaleX -= 0.1;
                this.layoutTransform.ScaleY -= 0.1;
            }
        }

        //Gestion du DragScroll

        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;
        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(this.ScrollViewerGraphe);
            hOff = this.ScrollViewerGraphe.HorizontalOffset;
            vOff = this.ScrollViewerGraphe.VerticalOffset;
            this.ScrollContent.CaptureMouse();
        }

        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.ScrollContent.IsMouseCaptured)
            {
                this.ScrollViewerGraphe.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(this.ScrollViewerGraphe).X));
                this.ScrollViewerGraphe.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(this.ScrollViewerGraphe).Y));
            }
        }

        private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ScrollContent.ReleaseMouseCapture();
        }
    }
}
