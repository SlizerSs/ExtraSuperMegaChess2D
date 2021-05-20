using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExtraSuperMegaChess2D
{
    class PlayerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            string d = (string)value;
            if (d == "w")
                return "Белый";
            else
                return "Чёрный";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string d = (string)value;
            if (d == "Белый")
                return "w";
            else
                return "b";
        }
    }
}
