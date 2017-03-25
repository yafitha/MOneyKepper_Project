using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using MoneyKepper2.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public class ReportDetailsViewModel : NavigableViewModel, IReportDetailsVewModel
    {
        #region Members
        private IDataService DataService { get; set; }

        public DateTime StartDate
        {
            get; set;
        }

        public DateTime EndDate
        { get; set; }

        #endregion

        #region Bindable Properties

        private string _reportInfo;
        public string ReportInfo
        {
            get { return _reportInfo; }
            set { this.Set(ref _reportInfo, value); }
        }

        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { this.Set(ref _subTitle, value); }
        }

        public ObservableCollection<TransactionItem> AllTransactionsItems { get; set; }

        #endregion

        #region Commands


        #endregion

        #region Cotr's
        public ReportDetailsViewModel(IDataService dataService)
        {
            this.DataService = dataService;
            this.SetCommands();
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
        }

        #endregion

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.SubTitle = (string)args["SubTitle"];
                var startTime = (DateTime)args["StartDateTime"];
                var endTime = (DateTime)args["EndDateTime"];
                if (startTime == this.StartDate && this.EndDate == endTime)
                    return;

                this.StartDate = startTime;
                this.EndDate = endTime;
                var allTransactionsItems = this.DataService.GetTransactionsByDateAndType(StartDate, EndDate, null).Select(tran => new TransactionItem(tran, tran.Category)).OrderBy(t => t.Transaction.Date).ToList();
                this.AllTransactionsItems = new ObservableCollection<TransactionItem>(allTransactionsItems);
                this.ReportInfo = string.Format("דוח מתאריך {0} עד תאריך {1}", StartDate.ToString("dd/MM/yy"), EndDate.ToString("dd/MM/yy"));
            }
        }

        #endregion
    }
}
