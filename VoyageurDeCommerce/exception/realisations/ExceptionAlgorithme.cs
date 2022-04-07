using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.exception.realisations
{
    /// <summary>Exception levée en cas de problème d'algorithme</summary>
    public class ExceptionAlgorithme : ExceptionVdC
    {
        /// <summary>Constructeur</summary>
        /// <param name="message">Message à afficher</param>
        public ExceptionAlgorithme(string message) : base("Problème d'algorithme", message)
        {
        }
    }
}
