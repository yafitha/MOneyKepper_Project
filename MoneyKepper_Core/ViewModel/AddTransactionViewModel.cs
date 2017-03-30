using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyKepper2.Service;
using MoneyKepper_Core.BL;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.ViewModel
{
    class AddTransactionViewModel : DialogViewModel, IAddTransactionViewModel
    {
        #region Members

        private IDataService DataService { get; set; }
        #endregion

        #region RaisePropertyChanged Members

        private bool _addNewCategory;
        public bool AddNewCategory
        {
            get { return _addNewCategory; }
            set
            {
                this.Set(ref _addNewCategory, value);
            }
        }

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

        private ObservableCollection<Category> _categoriesToCreateSubCategory;
        public ObservableCollection<Category> CategoriesToCreateSubCategory
        {
            get { return _categoriesToCreateSubCategory; }
            set { this.Set(ref _categoriesToCreateSubCategory, value); }
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

        private string _validationMessage;
        public string ValidationMessage
        {
            get { return _validationMessage; }
            set { this.Set(ref _validationMessage, value); }
        }

        private string _validationCategoryMessage;
        public string ValidationCategoryMessage
        {
            get { return _validationCategoryMessage; }
            set { this.Set(ref _validationCategoryMessage, value); }
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

        private DateTimeOffset? _currentMonth;
        public DateTimeOffset? CurrentMonth
        {
            get { return _currentMonth; }
            set { this.Set(ref _currentMonth, value); }
        }

        private ObservableCollection<ImageItem> _images;
        public ObservableCollection<ImageItem> Images
        {
            get { return _images; }
            set { this.Set(ref _images, value); }
        }
        private ImageItem _SelectedImage;
        public ImageItem SelectedImage
        {
            get { return _SelectedImage; }
            set { this.Set(ref _SelectedImage, value); }
        }
        public Types CategoryType { get; private set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.Set(ref _name, value); }
        }

        private bool _isIncome;
        public bool IsIncome
        {

            get { return _isIncome; }
            set { this.Set(ref _isIncome, value); }
        }

        private bool _isSubCategoryChecked;
        public bool IsSubCategoryChecked
        {
            get { return _isSubCategoryChecked; }
            set { this.Set(ref _isSubCategoryChecked, value); }
        }


        private Category _selectedSubCategory;
        public Category SelectedSubCategory
        {
            get { return _selectedSubCategory; }
            set { this.Set(ref _selectedSubCategory, value); }
        }

        #endregion

        #region Commands
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand AddCategoryCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public Action<TransactionItem> CallBack { get; private set; }
        public RelayCommand SaveCategoryCommand { get; private set; }
        public RelayCommand CloseCategoryCommand { get; private set; }
        public RelayCommand<string> CategoryTypeSelectionCommand { get; set; }
        public Action<Category> AddNewCategoryCallback { get; private set; }


        #endregion

        #region Constructors

        public AddTransactionViewModel(IDataService dataService)
        {
            this.DataService = dataService;
            this.SetCommands();
        }

        #endregion

        #region Private Methods

        private void SetCommands()
        {
            this.CategoryTypeSelectionCommand = new RelayCommand<string>(OnCategoryTypeSelectionCommand);
            this.CloseCommand = new RelayCommand(OnCloseCommand);
            this.SaveCommand = new RelayCommand(OnSaveCommand);
            this.SaveCategoryCommand = new RelayCommand(OnSaveCategoryCommand);
            this.CloseCategoryCommand = new RelayCommand(OnCloseCategoryCommand);
            this.AddCategoryCommand = new RelayCommand(OnAddCategoryCommand);
        }

        private void OnCloseCategoryCommand()
        {
            this.AddNewCategory = false;
        }
        private void OnCategoryTypeSelectionCommand(string type)
        {
            this.CategoryType = (Types)Enum.Parse(typeof(Types), type);
            var types = new List<Types>();
            types.Add(this.CategoryType);
            this.SetCategories(types);
        }

        private void SetCategories(List<Types> types)
        {
            this.CategoriesToCreateSubCategory = new ObservableCollection<Category>(this.DataService.GetCategoriesByTypes(types).Where(s => s.IsParent));
        }

        private void OnSaveCategoryCommand()
        {
            if (this.Name == string.Empty)
            {
                this.ValidationCategoryMessage = "יש להכניס שם קטגוריה";
                return;
            }
            if (this.SelectedImage == null)
            {
                this.ValidationCategoryMessage = "יש לבחור תמונה";
                return;
            }
            var type = this.IsIncome == true ? 1 : 2;
            var category = new Category(this.Name, type, this.SelectedImage.Path, !this.IsSubCategoryChecked, this.SelectedSubCategory?.ID);
            var returenedResult = CategoryBL.CreateNewCategory(category);
            if (returenedResult.Item1 == false)
                return;

            if ((this.Categories.FirstOrDefault().TypeID == returenedResult.Item2.TypeID))
            {
                this.Categories.Add(returenedResult.Item2);
                this.SelectedCategory = returenedResult.Item2;
            }
            this.AddNewCategoryCallback(returenedResult.Item2);
            this.AddNewCategory = false;
        }

        #endregion

        #region Commands Handlers


        private void OnAddCategoryCommand()
        {
            this.AddNewCategory = true;
            this.SetImages();
        }
        private void SetImages()
        {
            this.Images = new ObservableCollection<ImageItem>(this.DataService.GetAvailableImages().Select(s => new ImageItem(s)));
        }

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
            Transaction transaction = new Transaction() { Amount = double.Parse(this.Amount), CategoryID = SelectedCategory.ID, Note = this.Note, Date = this.CurrentMonth.Value.DateTime };
            this.CallBack(new TransactionItem(transaction, SelectedCategory));
            this.Hide();
        }

        private void OnCloseCommand()
        {
            this.Hide();
        }
        override public void OnShow(object parameter)
        {
            this.AddNewCategory = false;
            this.TransactionType = (parameter as Dictionary<string, object>)["TransactionType"] as string;
            this.Categories = new ObservableCollection<Category>((parameter as Dictionary<string, object>)["Categories"] as List<Category>);
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<TransactionItem>;
            this.AddNewCategoryCallback = (parameter as Dictionary<string, object>)["AddNewCategoryCallback"] as Action<Category>;
            this.CurrentMonth = (DateTime)(parameter as Dictionary<string, object>)["CurrentMonth"];
            this.Note = string.Empty;
            this.Amount = string.Empty;
            var startTime = new DateTime(CurrentMonth.Value.DateTime.Year, CurrentMonth.Value.DateTime.Month, 1);
            this.StartDate = startTime;
            this.EndDate = startTime.AddMonths(1).AddDays(-1);
            this.ValidationMessage = "";
            this.ValidationCategoryMessage = "";
            this.Name = "";
        }

        #endregion
    }
}
