using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper2.Converters
{
    public class TypeIDToTypeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return value;

            var typeId = (int)value;
            if (typeId == 1)
            {
                return "הכנסות";
            }

            return "הוצאות";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
