using MoneyKepper_Core.Models;
using MoneyKepper_Core.Services;
using MoneyKepper_Core.ViewModel;
using MoneyKepper2.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper2
{
    public class DialogService : GalaSoft.MvvmLight.Views.DialogService, IDialogService
    {
        private readonly Dictionary<string, Type> _dialogIndex = new Dictionary<string, Type>();
        private Dictionary<string, Type> DialogIndex
        {
            get { return _dialogIndex; }
        }

        public DialogService()
        {
            this.Configure(DialogKeys.ADD_TRANSACTION, typeof(AddTransactionDialog));
            this.Configure(DialogKeys.ADD_CATEGORY, typeof(AddNewCategoryDialog));
            this.Configure(DialogKeys.CONFIRM, typeof(ConfirmDialog));
        }

        public async Task<ContentDialogResult> ShowDialog(string dialogKey, object parameter)
        {
            var dialog = Activator.CreateInstance(this.DialogIndex[dialogKey]);
            (dialog as ContentDialogBase).OnShow(parameter);
            ContentDialogResult result = await (dialog as ContentDialogBase).ShowAsync();
            return result;
        }

        //public Task<ContentDialogResult> ShowMessageDialog(string title, string content, bool showSecondaryButton = true, SecondaryContent secondaryButtonContent = SecondaryContent.Cancel)
        //{
        //    Dictionary<string, object> args = new Dictionary<string, object>();
        //    if (showSecondaryButton)
        //    {
        //        args.Add("SecondaryButtonContent", secondaryButtonContent);
        //    }
        //    args.Add("Title", title);
        //    args.Add("Content", content);

        //    var result = this.ShowDialog("Confirm", args);
        //    return result;
        //}

        public void Configure(string key, Type dialogType)
        {
            this.DialogIndex.Add(key, dialogType);
        }

    }
}
