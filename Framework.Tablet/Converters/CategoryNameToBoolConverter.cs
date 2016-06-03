using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using IndiaRose.Data.Model;
using SharpDX.Direct2D1;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Framework.Tablet.Converters
{
    public class CategoryNameToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une category en un booleen
        /// Si la category est la category home alors le booleen est a false
        /// Sinon il est a true
        /// </summary>
        /// <param name="value">La category à convertir</param>
        /// <param name="targetType">Inutile</param>
        /// <param name="parameter">Inutile</param>
        /// <param name="language">Inutile</param>
        /// <returns>Le booleen correspondant à la category</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Category input = value as Category;

            if (input == null)
                return false;
            return input.Id != -1;
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
