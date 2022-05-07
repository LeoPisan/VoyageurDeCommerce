using System.Collections.Generic;
using VoyageurDeCommerce.exception.realisations;
using VoyageurDeCommerce.modele.algorithmes.realisations;
using VoyageurDeCommerce.modele.lieux;
using VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques.selections;
using VoyageurDeCommerce.modele.algorithmes.realisations.algoGenetiques;

namespace VoyageurDeCommerce.modele.algorithmes
{
    /// <summary> Fabrique des algorithmes </summary>
    public class FabriqueAlgorithme
    {
        /// <summary>
        /// Méthode de fabrication
        /// </summary>
        /// <param name="type">Type de l'algorithme à construire</param>
        /// <param name="listeLieux">Liste des lieux</param>
        /// <returns>L'algorithme créé</returns>
        public static Algorithme Creer(TypeAlgorithme type)
        {
            Algorithme algo;
            switch (type)
            {
                //case TypeAlgorithme.ALGOEXEMPLE: algo = new AlgoExemple(); break;
                case TypeAlgorithme.CROISSANT: algo = new AlgorithmeCroissant(); break;
                case TypeAlgorithme.INSERTION_AU_PLUS_PROCHE: algo = new InsertionAuPlusProche(); break;
                case TypeAlgorithme.PLUSPROCHEVOISIN: algo = new AlgoPlusProcheVoisin(); break;
                case TypeAlgorithme.INSERTION_AU_PLUS_LOIN: algo = new InsertionAuPlusLoin(); break;
                case TypeAlgorithme.VOISINAGE_TOURNEE: algo = new AlgoVoisinageTournee(); break; 
                case TypeAlgorithme.CHRISTOFIDES: algo = new Christofides(); break;
                case TypeAlgorithme.GENETIQUE_ROULETTE: algo = new AlgoGenetiqueRoulette(); break;
                case TypeAlgorithme.CHRISTOFIDES_VERSION_BRUTE: algo = new Christofides_VersionBrute(); break;

                default: throw new ExceptionAlgorithme("Vous n'avez pas modifié la fabrique des algorithmes !");
            }

            return algo;
        }
    }
}
