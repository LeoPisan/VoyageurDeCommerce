using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.parseur
{
    /// <summary>Parseur de fichier de graphe</summary>
    public class Parseur
    {
        /// <summary>Propriétés nécessaires</summary>
        private Dictionary<string, Lieu> listeLieux;
        public Dictionary<string, Lieu> ListeLieux => listeLieux;
        private List<Route> listeRoutes;
        public List<Route> ListeRoutes => listeRoutes;
        private string adresseFichier;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="nomDuFichier">Nom du fichier à parser</param>
        public Parseur(String nomDuFichier)
        {
            this.listeLieux = new Dictionary<string, Lieu>();
            this.listeRoutes = new List<Route>();
            this.adresseFichier = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\ressources\" + nomDuFichier;
        }

        /// <summary>
        /// Parsage du fichier
        /// </summary>
        public void Parser()
        {
            /*
            listeLieux.Add("1", new Lieu(TypeLieu.USINE, "1", 0, 0));
            listeLieux.Add("2", new Lieu(TypeLieu.MAGASIN, "2", 2, 0));
            listeLieux.Add("3", new Lieu(TypeLieu.MAGASIN, "3", -2, 2));
            listeLieux.Add("4", new Lieu(TypeLieu.MAGASIN, "4", 4, 2));
            listeLieux.Add("5", new Lieu(TypeLieu.MAGASIN, "5", 1, 4));

            listeRoutes.Add(new Route(listeLieux["1"], listeLieux["2"], 2));
            listeRoutes.Add(new Route(listeLieux["1"], listeLieux["3"], 3));
            listeRoutes.Add(new Route(listeLieux["1"], listeLieux["5"], 6));
            listeRoutes.Add(new Route(listeLieux["2"], listeLieux["4"], 1));
            listeRoutes.Add(new Route(listeLieux["2"], listeLieux["5"], 3));
            listeRoutes.Add(new Route(listeLieux["3"], listeLieux["5"], 4));
            listeRoutes.Add(new Route(listeLieux["4"], listeLieux["5"], 1));
            */

            using (StreamReader stream = new StreamReader(this.adresseFichier))
            {
                string ligne;
                while ((ligne = stream.ReadLine()) != null)
                {
                    string[] morceaux = ligne.Split(' ');
                    if (morceaux[0] == "ROUTE")
                        listeRoutes.Add(MonteurRoute.Creer(morceaux, listeLieux));
                    else
                        listeLieux.Add(morceaux[1],MonteurLieu.Creer(morceaux));
                }
            }
        }
    }
}
