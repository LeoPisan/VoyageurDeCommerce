using System.Collections.Generic;

namespace VoyageurDeCommerce.modele.lieux
{
    /// <summary> Lieu (magasin ou usine) </summary>
    public class Lieu
    {
        /// <summary>Type du lieu (Magasin ou usine)</summary>
        private TypeLieu type;
        public TypeLieu Type => type;

        /// <summary>Nom du lieu (clé primaire)</summary>
        private string nom;
        public string Nom => nom;

        /// <summary>Abscisse du lieu</summary>
        private int x;
        public int X => x;

        /// <summary>Ordonnée du lieu</summary>
        private int y;
        public int Y => y;


        /// <summary>Constructeur par défaut</summary>
        /// <param name="type">Type du lieu (Magasin ou usine)</param>
        /// <param name="nom">Nom du lieu</param>
        /// <param name="x">Absisse du lieu</param>
        /// <param name="y">Ordonnée du lieu</param>
        public Lieu(TypeLieu type, string nom, int x, int y)
        {
            this.type = type;
            this.nom = nom;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return this.Nom;
        }
    }
}
