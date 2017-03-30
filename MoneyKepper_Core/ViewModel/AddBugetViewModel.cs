using GalaSoft.MvvmLight.Command;
using Models;
using MoneyKepper_Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public class AddBugetViewModel : DialogViewModel, IAddBugetViewModel
    {
        #region Members

        public BugetItem BugetItem { get; private set; }
        public Action<BugetItem> CallBack { get; private set; }
        public bool IsAddBuget { get; private set; }

        #endregion

        #region RaisePropertyChanged Members

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { this.Set(ref _selectedCategory, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
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


        private string _title;
        public string Title
        {
            get { return _title; }
            set { this.Set(ref _title, value); }
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
            if (this.BugetItem == null)
            {
                Buget buget = new Buget() { Amount = double.Parse(this.Amount), CategoryID = SelectedCategory.ID, Note = this.Note, Date = this.CurrentMonth.Value.DateTime };
                this.CallBack(new BugetItem(buget, SelectedCategory));
                this.Hide();
                return;
            }
            this.BugetItem.Buget.Amount = double.Parse(this.Amount);
            this.BugetItem.Buget.Note = this.Note;
            this.CallBack(BugetItem);
            this.Hide();
        }

        private void InitProperties()
        {
            this.ValidationMessage = "";
            if (this.BugetItem == null)
            {
                this.Amount = "";
                this.IsAddBuget = true;
                this.Title = "הוסף תקציב ";
            }
            else
            {
                this.IsAddBuget = false;
                this.Amount = BugetItem.Buget.Amount.ToString();
                this.Categories = new ObservableCollection<Category>();
                this.Categories.Add(BugetItem.Buget.Category);
                this.SelectedCategory = this.Categories[0];
                this.Title = "עדכן תקציב ";
            }
        }

        private void OnCloseCommand()
        {
            this.Hide();
        }
        override public void OnShow(object parameter)
        {
            var args = (parameter as Dictionary<string, object>);
            this.Categories = new ObservableCollection<Category>(args["Categories"] as List<Category>);
            this.CallBack = args["Callback"] as Action<BugetItem>;
            this.CurrentMonth = args.ContainsKey("CurrentMonth") ?
                (DateTime?)args["CurrentMonth"] : null;
            this.BugetItem = args.ContainsKey("BugetItem ") ?
             args["BugetItem "] as BugetItem : null;
            this.Note = string.Empty;
            this.Amount = string.Empty;
            var startTime = new DateTime(CurrentMonth.Value.DateTime.Year, CurrentMonth.Value.DateTime.Month, 1);
            this.ValidationMessage = "";
            this.InitProperties();
        }

        #endregion
    }
}
