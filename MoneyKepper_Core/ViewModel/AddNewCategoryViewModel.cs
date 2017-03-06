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
        public IDataService DataService { get; set; }
        public Action<Category> CallBack { get; private set; }
        public Types CategoryType { get; private set; }

        #endregion

        #region Bindable properties

        private List<string> _images;
        public List<string> Images
        {
            get { return _images; }
            set { this.Set(ref _images, value); }
        }
        private string _SelectedImage;
        public string SelectedImage
        {
            get { return _SelectedImage; }
            set { this.Set(ref _SelectedImage, value); }
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

        private bool _isSubCategoryChecked;
        public bool IsSubCategoryChecked
        {
            get { return _isSubCategoryChecked; }
            set { this.Set(ref _isSubCategoryChecked, value); }
        }

        #endregion


        #region Commands

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand<string> CategoryTypeSelectionCommand { get; set; }

        #endregion


        public AddNewCategoryViewModel(IDataService dataService)
        {
            this.DataService = dataService;
        }
        public override void OnShow(object parameter)
        {
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<Category>;
            SetImages();
            this.SetCommands();
            var types = new List<Types>();
            types.Add(this.CategoryType);
            this.SetCategories(types);
        }

        private void SetCommands()
        {
            this.CategoryTypeSelectionCommand = new RelayCommand<string>(OnCategoryTypeSelectionCommand);
            this.CloseCommand = new RelayCommand(() => this.Hide());
            this.SaveCommand = new RelayCommand(OnSaveCommand);
        }

        private void OnSaveCommand()
        {
            this.CallBack(new Category(this.Name, (int)this.CategoryType, this.SelectedImage, !this.IsSubCategoryChecked, this.SelectedCategory?.ID));
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
            this.Categories = new ObservableCollection<Category>(this.DataService.GetCategoriesByTypes(types));
        }

        private void SetImages()
        {
            this.Images = new List<string>(this.DataService.GetAvailableImages());
        }
    }
}
