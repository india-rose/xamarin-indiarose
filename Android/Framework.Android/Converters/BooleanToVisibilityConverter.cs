using System;
using System.Globalization;
using System.Windows.Data;
using Android.Views;

namespace IndiaRose.Framework.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un bool�en en une visibilit� android
        /// Si le bool�en est � vrai, on retourne Visible, sinon Gone
        /// Si on veut la n�gation du bool�en, il faut ajouter "Negation" en 3�me argument
        /// </summary>
        /// <param name="value">Le bool�en � convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">"Negtion" si on souhaite avoir la n�gation</param>
        /// <param name="culture">Inutile</param>
        /// <returns>La visibilit� android correspondant au bool�en</returns>
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
        /// Non impl�ment�
        /// Retourne l'exception NotImplementedException
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}