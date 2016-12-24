using GalaSoft.MvvmLight.Command;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public class ControlSplitViewModel : ViewModelCore , IControlSplitViewModel
    {
        #region Members

        #endregion

        #region Bindable properties

        private string _date;
        public string Date
        {
            get { return _date; }
            set { this.Set(ref _date, value); }
        }

        private bool _isSplitViewPaneOpen;
        public bool IsSplitViewPaneOpen
        {
            get { return _isSplitViewPaneOpen; }
            set { Set(ref _isSplitViewPaneOpen, value); }
        }

        #endregion

        #region Commands

        public RelayCommand ToggleSplitViewPaneCommand { get; private set; }
        public RelayCommand CloseSplitViewPaneCommand { get; private set; }

        #endregion

        #region Constructors

        public ControlSplitViewModel()
        {
            SetCommands();
            this.IsSplitViewPaneOpen = false;
            this.Date = DateTime.Now.ToString("dddd MMMM yyyy");
        }

        private void SetCommands()
        {
            this.ToggleSplitViewPaneCommand = new RelayCommand(() => this.IsSplitViewPaneOpen = !this.IsSplitViewPaneOpen);
        }

        #endregion
    }
}
