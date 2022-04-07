using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.vue.composants
{
    /// <summary>
    /// Animation de coloriage des tournées
    /// </summary>
    public class AnimationColoration
    {
        /// <summary>Tournée à colorier</summary>
        private Tournee tournee;
        /// <summary>Timer pour la coloration</summary>
        private DispatcherTimer timer;
        /// <summary>Fenetre principale</summary>
        private FenetrePrincipale fenetre;
        /// <summary>Avancement dans la coloration</summary>
        private int position;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="tournee">Tournée à colorier</param>
        /// <param name="fenetre">Fenetre appelante</param>
        public AnimationColoration(Tournee tournee, FenetrePrincipale fenetre)
        {
            this.tournee = tournee;
            this.fenetre = fenetre;
            this.position = 0;
            this.timer = new DispatcherTimer(DispatcherPriority.Normal);
            this.timer.Interval = TimeSpan.FromMilliseconds(500);
            this.timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Tick du timer
        /// </summary>
        /// <param name="sender">L'animation</param>
        /// <param name="e">Le tick</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(position == tournee.ListeLieux.Count)
            {
                this.fenetre.CleanColoration();
                this.timer.Stop();
            }
            else
            {
                List<Route> routes = FloydWarshall.Chemin(tournee.ListeLieux[position], tournee.ListeLieux[(position + 1) % tournee.ListeLieux.Count]);
                this.fenetre.ColorierRoutes(routes);
                position++;
            }
        }

        /// <summary>
        /// Démarre l'animation
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Stop l'animation
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }
    }
}
