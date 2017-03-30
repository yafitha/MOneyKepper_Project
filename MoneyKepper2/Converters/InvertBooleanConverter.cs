using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MoneyKepper2.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            return (bool)value == true ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value == true ? true : false;
        }
    }
}
