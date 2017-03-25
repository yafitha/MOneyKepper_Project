using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public class ConfirmViewModel : DialogViewModel, IConfirmViewModel
    {
        #region Bindable properties

        private string _title;
        public string Title
        {
            get { return _title; }
            set { this.Set(ref _title, value); }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { this.Set(ref _content, value); }
        }

        #endregion


        #region  Commands

        public GalaSoft.MvvmLight.Command.RelayCommand CloseCommand { get; set; }
        public GalaSoft.MvvmLight.Command.RelayCommand OkCommand { get; set; }
        public Action CallBack { get; private set; }

        #endregion
        public override void OnShow(object parameter)
        {
            this.Title = (string)((parameter as Dictionary<string, object>)["Title"]);
            this.Content = (string)((parameter as Dictionary<string, object>)["Content"]);
            this.CallBack = (Action)((parameter as Dictionary<string, object>)["CallBack"]);
            this.CloseCommand = new GalaSoft.MvvmLight.Command.RelayCommand(() => this.Hide());
            this.OkCommand = new GalaSoft.MvvmLight.Command.RelayCommand(OnOkCommand);
        }

        private void OnOkCommand()
        {
            this.CallBack();
            this.Hide();
        }
    }
}
