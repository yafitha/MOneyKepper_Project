using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using Models;
using MoneyKepper2.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.ViewModel
{
    public class AddNewCategoryViewModel : DialogViewModel, IAddNewCategoryViewModel
    {
        #region Members

        private IDataService DataService { get; set; }
        public Action<Category> CallBack { get; private set; }
        public Types CategoryType { get; private set; }

        #endregion

        #region Bindable properties

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

        private string _title;
        public string Title
        {
            get { return _title; }
            set { this.Set(ref _title, value); }
        }

        private string _validationMessage;
        public string ValidationMessage
        {
            get { return _validationMessage; }
            set { this.Set(ref _validationMessage, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { this.Set(ref _categories, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { this.Set(ref _selectedCategory, value); }
        }

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

        private bool _isAddCategory;
        public bool IsAddCategory
        {
            get { return _isAddCategory; }
            set { this.Set(ref _isAddCategory, value); }
        }

        #endregion


        #region Commands

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand<string> CategoryTypeSelectionCommand { get; set; }
        public Category Category { get; private set; }

        #endregion

        public AddNewCategoryViewModel(IDataService dataService)
        {
            this.DataService = dataService;
        }
        public override void OnShow(object parameter)
        {
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<Category>;
            this.Category = (parameter as Dictionary<string, object>).ContainsKey("Category") ?
                (parameter as Dictionary<string, object>)["Category"] as Category : null;

            this.SetImages();
            this.SetCommands();
            var types = new List<Types>();
            types.Add(this.CategoryType);
            this.SetCategories(types);
            this.InitProperties();
        }

        private void InitProperties()
        {
            this.ValidationMessage = "";
            if (this.Category == null)
            {
                this.Name = string.Empty;
                IsSubCategoryChecked = false;
                this.IsIncome = false;
                this.Title = "הוסף קטגוריה";
                IsAddCategory = true;
            }
            else
            {
                IsAddCategory = false;
                this.Title = "עדכן קטגוריה";
                this.Name = Category.Name;
                this.IsIncome = Category.TypeID == 1 ? true : false;
                this.SelectedImage = this.Images.FirstOrDefault(im=>im.Path == Category.PictureName);
                if (Category.ParentID != null)
                {
                    this.SelectedCategory = this.Categories.FirstOrDefault(c => c.ID == Category.ParentID);
                }
            }
        }

        private void SetCommands()
        {
            this.CategoryTypeSelectionCommand = new RelayCommand<string>(OnCategoryTypeSelectionCommand);
            this.CloseCommand = new RelayCommand(() => this.Hide());
            this.SaveCommand = new RelayCommand(OnSaveCommand);
        }

        private void OnSaveCommand()
        {
            if(this.Name == string.Empty)
            {
                this.ValidationMessage = "יש להכניס שם קטגוריה";
                return;
            }
            if(this.SelectedImage == null)
            {
                this.ValidationMessage = "יש לבחור תמונה";
                return;
            }

            if (this.Category == null)
            {
                var type = this.IsIncome == true ? 1 : 2;
                this.CallBack(new Category(this.Name, type, this.SelectedImage.Path, !this.IsSubCategoryChecked, this.SelectedCategory?.ID));
                this.Hide();
                return;
            }
            this.Category.Name = this.Name;
            this.Category.PictureName = this.SelectedImage.Path;
            this.CallBack(this.Category);
            this.Hide();
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
            this.Categories = new ObservableCollection<Category>(this.DataService.GetCategoriesByTypes(types).Where(s=>s.IsParent));
        }

        private void SetImages()
        {
            this.Images= new ObservableCollection<ImageItem>(this.DataService.GetAvailableImages().Select(s => new ImageItem(s)));
            if(Category != null)
            {
                var imageItem = new ImageItem(Category.PictureName);
                this.Images.Add(imageItem);
            }
        }
    }
}
