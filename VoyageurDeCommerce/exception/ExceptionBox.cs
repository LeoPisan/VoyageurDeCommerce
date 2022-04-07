using System.Windows;
using VoyageurDeCommerce.exception;

namespace VoyageurDeCommerce.vue
{
    /// <summary> Classe d'affichage d'une exception </summary>
    public class ExceptionBox
    {
        /// <summary> Affiche un pop-up d'erreur pour l'exception</summary>
        /// <param name="exception">L'exception à gérer</param>
        public static void Show(ExceptionVdC exception)
        {
            MessageBox.Show(exception.Message, exception.NomException, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
