using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExtraSuperMegaChess2D
{
    public class StringToNullableDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            string d = (string)value;
            if (d!=null)
                return d;
            else
                return String.Empty;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string s = (string)value;
            if (String.IsNullOrEmpty(s))
                return null;
            else
                return s;
        }
    }
}
