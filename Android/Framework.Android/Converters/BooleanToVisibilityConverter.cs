using System;
using System.Globalization;
using System.Windows.Data;
using Android.Views;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un booléen en une visibilité android
        /// Si le booléen est à vrai, on retourne Visible, sinon Gone
        /// Si on veut la négation du booléen, il faut ajouter "Negation" en 3ème argument
        /// </summary>
        /// <param name="value">Le booléen à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">"Negtion" si on souhaite avoir la négation</param>
        /// <param name="culture">Inutile</param>
        /// <returns>La visibilité android correspondant au booléen</returns>
        /// <see cref="ViewStates"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string ?? "";
            bool res = (bool) value;
	        if (Equals(param, "Negation"))
	        {
		        res = !res;
	        }
	        return res ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <summary>
        /// Non implémenté
        /// Retourne l'exception NotImplementedException
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}