using GalaSoft.MvvmLight.Command;
using Models;
using MoneyKepper_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public class AddBugetViewModel : DialogViewModel , IAddBugetViewModel
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

        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { this.Set(ref _amount, value); }
        }

        private string _validationMessage;
        public string ValidationMessage
        {
            get { return _validationMessage; }
            set { this.Set(ref _validationMessage, value); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { this.Set(ref _note, value); }
        }


        private DateTimeOffset? _currentMonth;
        public DateTimeOffset? CurrentMonth
        {
            get { return _currentMonth; }
            set { this.Set(ref _currentMonth, value); }
        }

        #endregion

        #region Commands
        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }
        public Action<BugetItem> CallBack { get; private set; }

        #endregion

        #region Constructors

        public AddBugetViewModel()
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
            if (this.SelectedCategory == null)
            {
                this.ValidationMessage = "יש לבחור קטגוריה";
                return;
            }
            if (string.IsNullOrEmpty(this.Amount))
            {
                this.ValidationMessage = "יש להכניס סכום";
                return;
            }

            this.ValidationMessage = "";
            Buget buget = new Buget() { Amount = double.Parse(this.Amount) ,CategoryID = SelectedCategory.ID, Note = this.Note, Date = this.CurrentMonth.Value.DateTime };
            this.CallBack(new BugetItem(buget , SelectedCategory));
            this.Hide();
        }

        private void OnCloseCommand()
        {
            this.Hide();
        }
        override public void OnShow(object parameter)
        {
            this.Categories = (parameter as Dictionary<string, object>)["Categories"] as List<Category>;
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<BugetItem>;
            this.CurrentMonth = (DateTime)(parameter as Dictionary<string, object>)["CurrentMonth"];
            this.Note = string.Empty;
            this.Amount = string.Empty;
            var startTime = new DateTime(CurrentMonth.Value.DateTime.Year, CurrentMonth.Value.DateTime.Month, 1);
            this.ValidationMessage = "";
        }

        #endregion
    }
}
