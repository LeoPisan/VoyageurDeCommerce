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
    class Centauri : Algorithme
    {
        public override string Nom => "Centauri";

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

            // Initialisation des derniers lieux
            Lieu dernier1 = Tournee.ListeLieux[0];
            Lieu dernier2 = Tournee.ListeLieux[1];
            Lieu min;

            // Variables utiles
            int sommeDistanceCourant;
            int minDistance;

            while (!(tempLieux.Count <= 0))
            {                
                min = tempLieux[0];
                minDistance = FloydWarshall.Distance(dernier1, dernier2) + 1;
                foreach (Lieu lieu in tempLieux)
                {
                    sommeDistanceCourant = FloydWarshall.Distance(lieu, dernier1) + FloydWarshall.Distance(lieu, dernier2);          
                    if (sommeDistanceCourant < minDistance)
                    {
                        minDistance = sommeDistanceCourant;
                        min = lieu;
                    }                    
                }

                // Ajoute le point le plus proche des deux derniers points ajoutés en passant par là où il ajoutera le moins de distance en plus
                int positionLieu = Outils.IndexLieuPlusProcheTournee(min, Tournee);
                this.Tournee.ListeLieux.Insert(positionLieu, min);
                dernier1 = dernier2;
                dernier2 = min;
                tempLieux.Remove(min);

                // Capture
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
