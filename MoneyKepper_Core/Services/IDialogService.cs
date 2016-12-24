using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper_Core.Services
{
    public interface IDialogService : GalaSoft.MvvmLight.Views.IDialogService
    {
        Task<ContentDialogResult> ShowDialog(string dialogKey, object parameter);
       // Task<ContentDialogResult> ShowMessageDialog(string title, string content, bool ShowSecondaryButton = true, SecondaryContent secondaryButtonContent = SecondaryContent.Cancel);
    }

    public class DialogShownEventArgs
    {
        public object Parameter { get; set; }
    }
}
