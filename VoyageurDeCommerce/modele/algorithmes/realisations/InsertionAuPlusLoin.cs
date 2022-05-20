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
            List<Lieu> aVisiter = Outils.OrganiseUsine(lieux);

            // Initialise la tournée
            Tournee.Add(aVisiter[0]);
            Tournee.Add(FloydWarshall.PlusLoin(Tournee.ListeLieux[0], aVisiter));
            aVisiter.Remove(Tournee.ListeLieux[0]);
            aVisiter.Remove(Tournee.ListeLieux[1]);

            // Initialisation des derniers lieux
            Lieu dernier1 = Tournee.ListeLieux[0];
            Lieu dernier2 = Tournee.ListeLieux[1];
            Lieu max;

            // Capture
            stopwatch.Stop();
            this.NotifyPropertyChanged("Tournee");
            stopwatch.Start();

            // Variables utiles
            int sommeDistanceCourant;
            int maxDistance;

            // Tant qu'il existe des lieux non visités
            while (aVisiter.Count > 0)
            {
                max = aVisiter[0];
                maxDistance = 0;

                // On récupère le lieux le plus loin de la tournée
                foreach (Lieu lieu in aVisiter)
                {
                    sommeDistanceCourant = FloydWarshall.DistanceCouple(lieu, dernier1, dernier2);
                    if (sommeDistanceCourant > maxDistance)
                    {
                        maxDistance = sommeDistanceCourant;
                        max = lieu;
                    }
                }                

                // On récupère l'index où l'insertion serait la plus optimale
                int indexOpti = Outils.IndexLieuPlusProcheTournee(max, Tournee);

                // On insère dans la tournée le max à l'index le plus opti
                Tournee.ListeLieux.Insert(indexOpti, max);

                // On enlève le liieu dans la liste à visiter
                aVisiter.Remove(max);
               
                // Capture
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }

            // Arrêt de la stopwatch et affichage
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
        }

    }
}
