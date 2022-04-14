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

        /// <summary>
        /// Consiste à insérer toujours un lieu à la position où il augmentera le moins le trajet de la tournée.
        /// </summary>
        /// <param name="lieux">Liste des lieux du graphe</param>
        /// <param name="routes">Liste des routes du graphe</param>
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
            Lieu[] couplePlusLoin = FloydWarshall.PlusLoin(tempLieux);
            Transfere(couplePlusLoin[0], tempLieux);
            Transfere(couplePlusLoin[1], tempLieux);

            // Initialisation des derniers lieux
            Lieu dernier1 = Tournee.ListeLieux[0];
            Lieu dernier2 = Tournee.ListeLieux[1];
            Lieu min;

            // Capture
            stopwatch.Stop();
            this.NotifyPropertyChanged("Tournee");
            stopwatch.Start();

            // Variables utiles
            int sommeDistanceCourant;
            int minDistance;

            // Tant que notre liste temporaire n'est pas vide
            while (!(tempLieux.Count <= 0))
            {
                min = tempLieux[0];
                minDistance = FloydWarshall.Distance(dernier1, dernier2) + 1;
                foreach (Lieu lieu in tempLieux)
                {
                    // Distance d’un lieu L à un couple de lieu (A,B)
                    sommeDistanceCourant = FloydWarshall.DistanceCouple(lieu, dernier1, dernier2);
                    
                    if (sommeDistanceCourant <= minDistance)
                    {
                        minDistance = sommeDistanceCourant;
                        min = lieu;
                    }
                }

                // Ajoute le point le plus proche des deux derniers points ajoutés en passant par là où il ajoutera le moins de distance en plus
                int positionLieu = Outils.IndexLieuPlusProcheTournee(min, Tournee);
                Tournee.ListeLieux.Insert(positionLieu, min);
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
            Tournee.ListeLieux.Insert(Tournee.ListeLieux.Count, Tournee.ListeLieux[0]);
            this.NotifyPropertyChanged("Tournee");
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
