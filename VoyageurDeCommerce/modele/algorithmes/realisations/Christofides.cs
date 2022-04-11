using System;
using System.Collections.Generic;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Exemple de classe Algorithme, à ne pas garder
    /// </summary>
    public class Christofides : Algorithme
    {

        public override string Nom => "Christofides";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            //Lancer FloydWarshall
            FloydWarshall.calculerDistances(listeLieux, listeRoute);


        }
    }
}