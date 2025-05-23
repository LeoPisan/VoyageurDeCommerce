﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.distances;
using System.Diagnostics;


namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Exemple de classe Algorithme, à ne pas garder
    /// </summary>
    public class Christofides_VersionBrute : Algorithme
    {
        private Stopwatch stopwatch = new Stopwatch();
        public override string Nom => "Christofides Version Brute";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            
            // Lancement de la stopwatch
            stopwatch.Reset();
            stopwatch.Start();

            // Lancer FloydWarshall
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // Affecte les lieux et routes dans des listes temporaires et les trie les routes par distances croissantes
            List<Route> listeRouteTemp = new List<Route>(listeRoute.OrderBy(Route => Route.Distance).ToList());
            
            // Arbre couvrant du graphe de base
            List<Route> routesArbreCouvrant = Kruskal(listeLieux, listeRouteTemp);

            // Lieux de degré impair de l'arbre couvrant
            List<Lieu> lieuxDegreImpair = ListeLieuDegreImpair(listeLieux, routesArbreCouvrant);

            // Supprime les routes ne menant plus à aucun lieu et les tris dans l'ordre coissant selon la distance
            List<Route> routeGrapheInduit = SupprimeRouteEnTrop(lieuxDegreImpair, listeRouteTemp).OrderBy(Route => Route.Distance).ToList();

            Console.WriteLine("\nGraphe induit :");
            Outils.AfficheRoute(routeGrapheInduit);

            // Couplage de poids minimum
            List<Route> couplageMinimal = Couplage(routeGrapheInduit, lieuxDegreImpair, routesArbreCouvrant);

            Console.WriteLine("\nCouplage :");
            Outils.AfficheRoute(couplageMinimal);

            // Union du couplage et de l'arbre couvrant
            List<Route> union = couplageMinimal.Union(routesArbreCouvrant).ToList();

            Console.WriteLine("\nUnion :");
            Outils.AfficheRoute(union);

            // Fait un tour eulerien de graphe union
            List<Lieu> final = TourEulerien(listeLieux, union);
            
            // Ajoute dans  la tournée chaque lieu du tour eulerien
            foreach(Lieu lieu in final)
            {
                Tournee.ListeLieux.Add(lieu);

                // Capture de la tournée
                stopwatch.Stop();
                this.NotifyPropertyChanged("Tournee");
                stopwatch.Start();
            }

            // Finis le cycle
            Outils.FinCycle(Tournee);
            stopwatch.Stop();

            // Capture de la tournée
            this.NotifyPropertyChanged("Tournee");
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
           
            
        }



        /// <summary>
        /// Fait un tour eulerien d'un graphe
        /// </summary>
        /// <param name="lieux">Lieux du graphe</param>
        /// <param name="routes">Routes du graphe</param>
        private List<Lieu> TourEulerien(List<Lieu> lieux, List<Route> routes) 
        {            
            // Initialisation
            List<Lieu> voisins;
            List<Lieu> res = new List<Lieu>();  // Création de la liste de résultat
            List<Route> routesTemp = new List<Route>(routes);  // Affecte les valeurs des paramètres dans des variables temporaires
            Route aRetirer;

            // Premier lieu 
            Lieu lieuTravail = lieux[0];  // Affecte le premier lieu comme lieu de travail
            res.Add(lieuTravail);  // Ajoute à res le lieu
            voisins = Outils.Voisins(lieuTravail, routesTemp);  // Calcule les voisins du lieu actuel

            // Retire la route entre le lieu actuel et le suivant
            routesTemp.Remove(routesTemp.Find(Route => ((Route.Depart == lieuTravail) && (Route.Arrivee == voisins[0])) || ((Route.Depart == voisins[0]) && (Route.Arrivee == lieuTravail))));

            // Boucle de travail
            bool enCours = true;
            while (enCours)
            {
                // Si le sommet actuel ne posssède pas de voisin
                if (voisins.Count < 1)
                {
                    enCours = false;
                }
                else
                {
                    lieuTravail = voisins[0];  // Affecte le lieu suivant comme lieu actuel
                    res.Add(lieuTravail);  // Ajoute à res le lieu
                    voisins = Outils.Voisins(lieuTravail, routesTemp);  // Calcule les voisins du lieu actuel

                    // Retire la route entre le lieu actuel et le suivant
                    aRetirer = routesTemp.Find(Route => ((Route.Depart == lieuTravail) && (Route.Arrivee == voisins[0])) || ((Route.Depart == voisins[0]) && (Route.Arrivee == lieuTravail)));
                    //Console.WriteLine(aRetirer.Depart.ToString() + " => " + aRetirer.Arrivee.ToString() + "  reste : ");
                    routesTemp.Remove(aRetirer);
                }
            }

            // Retire les doublons
            HashSet<Lieu> antiDuplicant = new HashSet<Lieu>(res);
            res = antiDuplicant.ToList();

            // Retourne le résultat
            return res;
        }




        /// <summary>
        /// Renvoie un couplage de poids minimal d'un graphe
        /// </summary>
        /// <param name="routes">Routes du graphe</param>
        /// <param name="lieux">Lieux du graphe</param>
        private List<Route> Couplage(List<Route> routes, List<Lieu> lieux, List<Route> routesArbre)
        {         
            List<Route> res = new List<Route>();
            List<Route> resTemp = new List<Route>();

            List<Lieu> lieuxTemp = new List<Lieu>();
            List<Route> routesTemp = new List<Route>(routes);


            foreach (Lieu lieu in lieux)
            {
                lieuxTemp.Add(lieu);
            }

            int quantite = 0;
            int valeur = FloydWarshall.Infini;

            int quantiteMax = quantite;
            int valeurMin = valeur;

            // On essaye d'ajouter chaque route dans tous les rdres possibles
            foreach (int[] i in Heap(routesTemp.Count))
            {
                // On réinitialise les listes temporaires
                lieuxTemp.Clear();
                foreach (Lieu lieu in lieux)
                {
                    lieuxTemp.Add(lieu);
                }

                resTemp.Clear();
                quantite = 0;
                valeur = FloydWarshall.Infini;


                // Pour prend les index du tableau i dans l'ordre
                foreach (int j in i)
                {
                    // 
                    if (lieuxTemp.Contains(routesTemp[j].Depart) && lieuxTemp.Contains(routesTemp[j].Arrivee))
                    {
                        lieuxTemp.Remove(routesTemp[j].Depart);
                        lieuxTemp.Remove(routesTemp[j].Arrivee);
                        resTemp.Add(routesTemp[j]);
                        quantite++;
                        valeur += routesTemp[j].Distance;
                    }
                }


                // Si la quantié de couple est plus grand que le cas précédent
                if (quantite > quantiteMax)
                {
                    quantiteMax = quantite;
                    res.Clear();
                    foreach (Route route in resTemp)
                    {
                        res.Add(route);
                    }
                    res = resTemp;
                }
                else if (quantite == quantiteMax)
                {
                    // Si la somme de tous les couples est plus petit que celle d'avant
                    if (valeur < valeurMin)
                    {
                        valeurMin = valeur;
                        res.Clear();
                        foreach (Route route in resTemp)
                        {
                            res.Add(route);
                        }
                        res = resTemp;
                    }
                }
            }
                        
            return res;
        }



        /// <summary>
        /// Donne une liste de toutes les permutation possible des éléments 1, 2, 3, ..., n d'un tableau de taille n
        /// </summary>
        /// <param name="taille">Taille du tableau</param>
        /// <returns></returns>
        private List<int[]> Heap(int taille)
        {
            // Initialisation variables utiles
            int n = taille;
            List<int[]> res = new List<int[]>();
            int a;


            // On initialise tab avec les les valeurs 1, 2, 3, ..., n
            int[] tab = new int[n];            
            for (int j = 0; j < tab.Length; j++)
            {
                tab[j] = j;
            }
            
            // On initialise compteur qu'avec des 0
            int[] compteur = new int[n];
            for (int j = 0; j < tab.Length; j++)
            {
                compteur[j] = 0;
            }

            int[] tab2 = new int[n];
            tab.CopyTo(tab2, 0);
            res.Add(tab2);

            // i indique le niveau de la boucle en cours d'incrémentation
            int i = 0;

            while(i < n)
            {
                if (compteur[i] < i)
                {
                    if (i%2 == 0)
                    {
                        a = tab[0];
                        tab[0] = tab[i];
                        tab[i] = a;
                    }
                    else
                    {
                        a = tab[i];
                        tab[i] = tab[compteur[i]];
                        tab[compteur[i]] = a;
                    }

                    int[] tab3 = new int[n];
                    tab.CopyTo(tab3, 0);
                    res.Add(tab3);

                    compteur[i]++;
                    i = 0;
                }
                else
                {
                    compteur[i] = 0;
                    i++;
                }
            }

            return res;
        }
    


        /// <summary>
        /// Renvoie une liste qui supprime les routes qui ne mène à aucun lieu d'une liste passée en paramètre
        /// </summary>
        /// <param name="lieux"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        private List<Route> SupprimeRouteEnTrop(List<Lieu> lieux, List<Route> routes)
        {
            List<Route> res = new List<Route>();
            foreach (Route route in routes)
            {                
                if (lieux.Contains(route.Depart) && lieux.Contains(route.Arrivee))
                {
                    res.Add(route);
                }
            }
            return res;
        }

 

        /// <summary>
        /// Renvoie une liste de Lieu ayant un nombre de voisins impair
        /// </summary>
        /// <param name="listeLieux"></param>
        /// <param name="listeRoute"></param>
        private List<Lieu> ListeLieuDegreImpair(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            List<Lieu> res = new List<Lieu>();
            foreach (Lieu lieu in listeLieux)
            {
                if (Outils.NombreVoisins(lieu, listeRoute) % 2 != 0)
                {
                    res.Add(lieu);
                }
            }
            return res;
        }


        /// <summary>
        /// Algorithme de Kruskal qui renvoit les routes de l'arbre couvrant d'un graphe passé en paramètre via les lieux et routes
        /// </summary>
        /// <param name="lieux"></param>
        /// <param name="routes"></param>
        private List<Route> Kruskal(List<Lieu> lieux, List<Route> routes)
        {
            List<Route> res = new List<Route>();

            // Initialise une composante connexe pour chaque lieux
            Dictionary<Lieu, int> listeComposanteConnexe = new Dictionary<Lieu, int>();
            int i = 1;
            foreach (Lieu lieu in lieux)
            {
                listeComposanteConnexe.Add(lieu, i);
                i++;
            }

            // Initialisations de variables utiles
            Lieu lieu1;
            Lieu lieu2;
            int changement;

            // Algorithme de Kruskal
            foreach (Route route in routes)
            {
                lieu1 = route.Depart;
                lieu2 = route.Arrivee;
                if (listeComposanteConnexe[lieu1] != listeComposanteConnexe[lieu2])
                {
                    // On ajoute à notre arbre couvrant la route travaillée
                    res.Add(route);

                    // On fusionne les composantes connexes
                    changement = listeComposanteConnexe[lieu2];
                    foreach (Lieu lieu in lieux)
                    {
                        if (listeComposanteConnexe[lieu] == changement)
                        {
                            listeComposanteConnexe[lieu] = listeComposanteConnexe[lieu1];
                        }
                    }
                }
            }
            return res;
        }


   
    }
}