using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TaxiSupport
{
    internal class ImageTariffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tariff tariff = value as Tariff;
            string path = tariff.image as string;
            if (File.Exists(path))
            {
                return path = Path.GetFullPath(path);
            }
            else return "../../Resource/" + path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
