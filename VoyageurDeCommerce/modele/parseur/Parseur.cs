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
