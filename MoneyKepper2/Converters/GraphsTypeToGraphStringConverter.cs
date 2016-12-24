using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MoneyKepper2.Converters
{
    public class GraphsTypeToGraphStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return value;

            if (value is Graph)
            {
                Graph graphType = (Graph)value;
                switch (graphType)
                {
                    case Graph.CategoriesColumns:
                        return "עמודות";

                    case Graph.CategoriesMonthColumns:
                        return "קטגוריות";

                    case Graph.IncomeExpenses:
                        return "הוצאות-הכנסות";

                    case Graph.pie:
                        return "עוגה";
                    default:
                        return "קטגוריה";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
