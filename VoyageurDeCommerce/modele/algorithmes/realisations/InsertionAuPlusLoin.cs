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

        /// <summary>
        /// Fonctionne comme le précédent à la différence qu’au lieu de viser à ajouter le lieu le plus proche de la tournée, il ajoute le lieu le plus loin de cette dernière.
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
            Transfere(tempLieux[0], tempLieux);
            Transfere(FloydWarshall.PlusLoin(Tournee.ListeLieux[0], tempLieux), tempLieux);

            // Initialisation des derniers lieux
            Lieu dernier1 = Tournee.ListeLieux[0];
            Lieu dernier2 = Tournee.ListeLieux[1];
            Lieu max;

            // Variables utiles
            int sommeDistanceCourant;
            int maxDistance;

            while (!(tempLieux.Count <= 0))
            {
                max = tempLieux[0];
                maxDistance = 0;
                foreach (Lieu lieu in tempLieux)
                {
                    sommeDistanceCourant = FloydWarshall.Distance(lieu, dernier1) + FloydWarshall.Distance(lieu, dernier2);
                    if (sommeDistanceCourant > maxDistance)
                    {
                        maxDistance = sommeDistanceCourant;
                        max = lieu;
                    }
                }

                // Ajoute le point le plus loin des deux derniers points ajoutés en passant par là où il ajoutera le moins de distance en plus
                int positionLieu = Outils.IndexLieuPlusProcheTournee(max, Tournee);
                this.Tournee.ListeLieux.Insert(positionLieu, max);
                dernier1 = dernier2;
                dernier2 = max;
                tempLieux.Remove(max);

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
