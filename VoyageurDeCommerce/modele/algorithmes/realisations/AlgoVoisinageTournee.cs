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
            // Initialise et lance la stopwatch 
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //on part d'une tournée de base
            //on regarde si une des voisines de la tournée est meilleure que celle-ci
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            bool fin = false;
            List<Lieu> aTester = listeLieux;
            while (!fin)
            {
                this.Tournee.ListeLieux = compareVoisine(aTester); //si on en trouve une on remplace la tournée par la nouvelle, plus performante
                if (this.Tournee.ListeLieux == aTester)
                    fin = true; //si on n'en trouve pas on arrête et on renvoie la dernière trouvée
                aTester = this.Tournee.ListeLieux;
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
            stopwatch.Start();
            this.Tournee.Add(this.Tournee.ListeLieux[0]);
            stopwatch.Stop();
            this.NotifyPropertyChanged("Tournee");
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            
        }

        /// <summary>
        /// compare une tournée avec ses voisines et renvoie la plus courte
        /// </summary>
        /// <param name="aTester">tournée à tester</param>
        /// <returns>tournée la plus courte parmi les voisines</returns>
        private List<Lieu> compareVoisine(List<Lieu> aTester)
        {
            List<Lieu> temp = aTester;
            List<List<Lieu>> voisines = genereVoisines(aTester);
            foreach(List<Lieu> v in voisines)
            {
                if (new Tournee(temp).Distance > new Tournee(v).Distance)
                {
                    temp = v;
                }
            }
            return temp;
        }

        /// <summary>
        /// génère une liste de chemins voisins à celui entré en paramètre
        /// </summary>
        /// <param name="aTester">liste de lieux à tester</param>
        /// <returns>liste de chemins voisins</returns>
        private List<List<Lieu>> genereVoisines(List<Lieu> aTester)
        {
            List<List<Lieu>> temp = new List<List<Lieu>>();
            for (int i = 0; i < aTester.Count - 1; i++)
            {
                temp.Add(Outils.InverseElements(i, i + 1, aTester));
            }
            temp.Add(Outils.InverseElements(0, aTester.Count - 1, aTester));

            return temp;
        }
    }
}
