using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MoneyKepper2.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is DateTime)
                {
                    return ((DateTime)value).ToString(parameter.ToString());
                }
                return ((DateTimeOffset)value).Date.ToString(parameter.ToString());
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (value is DateTime)
                    return ((DateTime)value).ToString("yyyyMMdd");

                if (value is CalendarViewDayItem)
                    return (((CalendarViewDayItem)value).Date.DateTime).ToString("yyyyMMdd");
            }

            return string.Empty;
        }
    }
}
