using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    class AlgoVoisinageTournee : Algorithme
    {
        public override string Nom => "Voisinage d'une tournée";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            this.Tournee = new Tournee(listeLieux);
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            List<Lieu> tempListe = listeLieux; //liste temporaire destinée à devenir la tournée
            bool fin = false;
            while (!fin)
            {
                fin = true;
                foreach (Lieu l in tempListe) //on teste toutes les tournées voisines
                {
                    int indexA = tempListe.IndexOf(l);
                    List<Lieu> testListe = Outils.InverseElements(indexA, indexA + 1, tempListe);
                    if (this.Tournee.Distance > new Tournee(testListe).Distance) //si la tournée voisine est plus courte alors on la prend à la place de l'ancienne
                    {
                        this.Tournee = new Tournee(testListe);
                        fin = false;
                    }
                }
            }
            stopwatch.Stop();

            //petite modification pour l'affichage de la tournée
            Tournee retour = this.Tournee;
            this.Tournee = new Tournee();
            foreach (Lieu l in retour.ListeLieux)
            {
                this.Tournee.Add(l);
                this.NotifyPropertyChanged("Tournee");
            }
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
        }
    }
}
