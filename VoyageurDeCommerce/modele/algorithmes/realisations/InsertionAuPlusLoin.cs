using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;
using System.Diagnostics;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class InsertionAuPlusLoin : Algorithme
    {
        public override string Nom => "Insertion au plus loin";

        public override void Executer(List<Lieu> lieux, List<Route> routes)
        {
            // Initialisation et lancement de la stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Calcule des distances
            FloydWarshall.calculerDistances(lieux, routes);

            // Création variable temporaire contenant les lieux
            List<Lieu> tempLieux = Outils.OrganiseUsine(lieux);

            // Initialise la tournée
            Transfere(tempLieux[0], tempLieux);
            Transfere(FloydWarshall.PlusLoin(Tournee.ListeLieux[0], tempLieux), tempLieux);

            // Ajout de tout les lieux dans la tournee
            foreach (Lieu lieu in tempLieux)
            {
                int positionLieu = Outils.IndexLieuPlusLoinTournee(lieu, Tournee);
                this.Tournee.ListeLieux.Insert(positionLieu, lieu);
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }

            // Arrêt de la stopwatch et affichage
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
        }


        /// <summary>
        /// Transfere un lieu d'une liste de lieu à la tournee
        /// </summary>
        /// <param name="A"></param>
        /// <param name="listeLieux"></param>
        public void Transfere(Lieu A, List<Lieu> listeLieux)
        {
            this.Tournee.Add(A);
            listeLieux.Remove(A);
        }

    }
}
