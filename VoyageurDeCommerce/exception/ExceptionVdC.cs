using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoyageurDeCommerce.exception
{
    ///<summary> Classe d'exception personnalisée </summary>
    public abstract class ExceptionVdC : Exception
    {
        /// <summary>Nom de l'exception</summary>
        private readonly String nomException;
        public String NomException => nomException;

        /// <summary>Constructeur par défaut</summary>
        /// <param name="nomException">Nom de l'exception</param>
        /// <param name="message">Message de l'exception</param>
        public ExceptionVdC(String nomException, String message) : base(message)
        {
            this.nomException = nomException;
        }

    }
}
