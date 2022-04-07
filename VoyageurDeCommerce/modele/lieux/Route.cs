namespace VoyageurDeCommerce.modele.lieux
{
    /// <summary>Route entre deux lieux</summary>
    public class Route
    {
        /// <summary>Lieu de départ</summary>
        private Lieu depart;
        public Lieu Depart => depart;
        /// <summary>Lieu d'arrivée</summary>
        private Lieu arrivee;
        public Lieu Arrivee => arrivee;
        /// <summary>Longueur de la route</summary>
        private int distance;
        public int Distance => distance;

        /// <summary>Constructeur par défaut</summary>
        /// <param name="depart">Lieu de départ</param>
        /// <param name="arrivee">Lieu d'arrivée</param>
        /// <param name="distance">Distance entre les deux lieux</param>
        public Route(Lieu depart,Lieu arrivee,int distance)
        {
            this.depart = depart;
            this.arrivee = arrivee;
            this.distance = distance;
        }
    }
}
