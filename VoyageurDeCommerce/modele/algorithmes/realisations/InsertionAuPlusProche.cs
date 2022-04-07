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
    class InsertionAuPlusProche : Algorithme
    {
        public override string Nom => "Insertion au plus proche";

        public override void Executer(List<Lieu> lieux, List<Route> routes)
        {
            // Initialisation et lancement de la stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Calcule des distances
            FloydWarshall.calculerDistances(lieux, routes);

            // Création variable temporaire contenant les lieux
            List<Lieu> tempLieux = lieux;

            // Initialise la tournée
            Transfere(tempLieux[0], tempLieux);
            Transfere(FloydWarshall.PlusLoin(Tournee.ListeLieux[0], tempLieux), tempLieux);

            Console.WriteLine(Tournee.ToString());

            
            foreach (Lieu lieu in tempLieux)
            {
                int positionLieu = FloydWarshall.IndexLieuPlusProcheTournee(lieu, Tournee);
                this.Tournee.ListeLieux.Insert(positionLieu, lieu);
                Console.WriteLine(Tournee.ToString());
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }
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
