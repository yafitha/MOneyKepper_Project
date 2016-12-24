using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    class AddTransactionViewModel : DialogViewModel, IAddTransactionViewModel
    {
        #region Members

        #endregion

        #region RaisePropertyChanged Members

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { this.Set(ref _selectedCategory, value); }
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get { return _categories; }
            set { this.Set(ref _categories, value); }
        }

        private string _transactionType;
        public string TransactionType
        {
            get { return _transactionType; }
            set { this.Set(ref _transactionType, value); }
        }

        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { this.Set(ref _amount, value); }
        }
        private string _note;
        public string Note
        {
            get { return _note; }
            set { this.Set(ref _note, value); }
        }

        private DateTimeOffset? _startDate;
        public DateTimeOffset? StartDate
        {
            get { return _startDate; }
            set { this.Set(ref _startDate, value); }
        }

        private DateTimeOffset? _endDate;
        public DateTimeOffset? EndDate
        {
            get { return _endDate; }
            set { this.Set(ref _endDate, value); }
        }

        private DateTimeOffset? _date;
        public DateTimeOffset? Date
        {
            get { return _date; }
            set { this.Set(ref _date, value); }
        }

        #endregion

        #region Commands
        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }
        public Action<TransactionItem> CallBack { get; private set; }

        #endregion

        #region Constructors

        public AddTransactionViewModel()
        {
            this.SetCommands();
        }

        #endregion

        #region Private Methods

        private void SetCommands()
        {
            this.CloseCommand = new RelayCommand(OnCloseCommand);
            this.SaveCommand = new RelayCommand(OnSaveCommand);
        }

        #endregion

        #region Commands Handlers

        private void OnSaveCommand()
        {
            Transaction transaction = new Transaction() { Amount = double.Parse(this.Amount), CategoryID = SelectedCategory.ID, Note = this.Note, Date = this.Date.Value.DateTime };
            this.CallBack(new TransactionItem(transaction, SelectedCategory));
            this.Hide();
        }

        private void OnCloseCommand()
        {
            this.Hide();
        }
        override public void OnShow(object parameter)
        {
            this.TransactionType = (parameter as Dictionary<string, object>)["TransactionType"] as string;
            this.Categories = (parameter as Dictionary<string, object>)["Categories"] as List<Category>;
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<TransactionItem>;
            this.Note = string.Empty;
            this.Amount = string.Empty;
            DateTime now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, 1);
            this.StartDate = startTime;
            this.EndDate = startTime.AddMonths(1).AddDays(-1);
            this.Date = now;
        }

        #endregion
    }
}
