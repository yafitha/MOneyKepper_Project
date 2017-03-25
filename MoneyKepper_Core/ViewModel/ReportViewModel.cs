using GalaSoft.MvvmLight.Command;
using MoneyKepper2.Service;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public class ReportViewModel : NavigableViewModel, IReportViewModel
    {
        #region Members

        private IDataService DataServcie { get; set; }
        private IActionsService ActionsService { get; set; }

        #endregion

        #region Commands

        public RelayCommand ShowReportCommand { get; set; }

        #endregion

        #region Bindable Properties
        private DateTimeOffset? _startDate;
        public DateTimeOffset? StartDate
        {
            get { return _startDate; }
            set
            {
                this.Set(ref _startDate, value);
                if (value > this.EndDate)
                {
                    this.EndDate = value;
                }
            }
        }

        private DateTimeOffset? _endDate;
        public DateTimeOffset? EndDate
        {
            get { return _endDate; }
            set { this.Set(ref _endDate, value); }
        }

        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { this.Set(ref _subTitle, value); }
        }
        
        #endregion

        #region Ctor's
        public ReportViewModel(IDataService dataService, IActionsService actionsService)
        {
            this.DataServcie = dataService;
            this.ActionsService = actionsService;
            this.SetCommands();
        }

        #endregion


        #region Methods
        private void SetCommands()
        {
            this.ShowReportCommand = new RelayCommand(OnShowReportCommand);
        }

        private void OnShowReportCommand()
        {
            var endDate = this.EndDate == null ? this.StartDate.Value.Date : this.EndDate.Value.Date;
            this.ActionsService.ShowReportDetails(this.StartDate.Value.DateTime, endDate, SubTitle);
        }

        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.StartDate = DateTime.Today;
                this.EndDate = null;
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
    }
}

