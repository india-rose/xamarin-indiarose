using System;
using Windows.UI.Xaml.Data;
using IndiaRose.Data.Model;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace IndiaRose.Framework.Converters
{
    public class ParentCategoryTextConverter : IValueConverter
    {
        public string Uid { get; set; }

        public string Property { get; set; }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            Indiagram input = value as Indiagram;

            if (input == null)
            {
                if (Property == null)
                {
                    return LazyResolver<ILocalizationService>.Service.GetString(Uid);
                }

                return LazyResolver<ILocalizationService>.Service.GetString(Uid, Property);
            }
            return input.Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
