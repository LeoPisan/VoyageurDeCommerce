using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.vue.ressources;

namespace VoyageurDeCommerce.vue.composants
{
    /// <summary>
    /// Logique d'interaction pour PanelTournee.xaml
    /// </summary>
    public partial class PanelTournee : UserControl
    {

        private Tournee tournee;
        public Tournee Tournee => tournee;
        private int numero;

        public PanelTournee(Tournee tournee,int numero)
        {
            InitializeComponent();
            this.tournee = tournee;
            this.numero = numero;
            this.ImageTournee.Source = ImagesManager.GetImage("TOURNEE");
            this.TextNom.Text = "Tournée n°" + (numero+1).ToString();
            this.TextDistance.Text = this.tournee.Distance.ToString();
        }

        public void Selectionner()
        {
            this.Background = Brushes.LightBlue;
        }

        public void Deselectionner()
        {
            this.Background = Brushes.Transparent;
        }
    }
}
