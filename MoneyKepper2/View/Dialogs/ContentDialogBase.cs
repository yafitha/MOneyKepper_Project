using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper2.View.Dialogs
{
    public class ContentDialogBase : ContentDialog
    {
        public ContentDialogBase()
        {
        }

        internal void OnShow(object parameter)
        {
            if (this.DataContext is IDialog)
            {
                (this.DataContext as IDialog).Initialize(() => this.Hide());
                (this.DataContext as IDialog).OnShow(parameter);
            }
        }
    }
}
