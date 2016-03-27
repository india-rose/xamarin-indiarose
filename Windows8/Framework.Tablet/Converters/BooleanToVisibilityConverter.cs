using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un booléen en une visibilité android
        /// Si le booléen est à vrai, on retourne Visible, sinon Collapsed
        /// Si on veut la négation du booléen, il faut ajouter "Negation" en 3ème argument
        /// </summary>
        /// <param name="value">Le booléen à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">"Negtion" si on souhaite avoir la négation</param>
        /// <param name="language">Inutile</param>
        /// <returns>La visibilité android correspondant au booléen</returns>
        /// <see cref="Visibility"/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string param = parameter as string ?? "";
            bool res = (bool) value;
            if (Equals(param, "Negation"))
            {
                res = !res;
            }
            return res ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Non implémenté
        /// Retourne l'exception NotImplementedException
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
