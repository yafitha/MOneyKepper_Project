using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.Models.Groups;
using Models;
using MoneyKepper_Core.Services;
using MoneyKepper2.Service;
using MoneyKepperCore.Service;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using MoneyKepper_Core.BL;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper_Core.ViewModel
{
    public class CategoryDetailViewModel : NavigableViewModel, ICategoryDetailViewModel
    {
        #region Members
        private IDialogService DialogService { get; set; }
        private IDataService DataService { get; set; }
        public IList<Category> Categories { get; set; }
        private IActionsService ActionsService { get; set; }

        private List<Transaction> AllTransactions { get; set; }

        #endregion

        #region Bindable Properties

        private ObservableCollection<CategoryGroup> _incomesGroups;
        public ObservableCollection<CategoryGroup> IncomesGroups
        {
            get { return _incomesGroups; }
            set { this.Set(ref _incomesGroups, value); }
        }

        private ObservableCollection<CategoryGroup> _expensesGroups;
        public ObservableCollection<CategoryGroup> ExpensesGroups
        {
            get { return _expensesGroups; }
            set { this.Set(ref _expensesGroups, value); }
        }

        #endregion

        #region Commands

        public RelayCommand AddCategoryCommand { get; private set; }
        public RelayCommand<CategoryModel> RemoveCommand { get; private set; }

        public RelayCommand<CategoryModel> UpdateCategoryCommand { get; private set; }
        public Action<Category> AddCallBack { get; private set; }
        public Action<Category> RemoveCallBack { get; private set; }


        #endregion

        #region Cotr's
        public CategoryDetailViewModel(IDialogService dialogService, IDataService dataService, IActionsService actionsService)
        {
            this.DialogService = dialogService;
            this.DataService = dataService;
            this.ActionsService = actionsService;
            this.SetCommands();
        }

        #endregion

        #region Commands Handlers


        private async void OnRemoveCommand(CategoryModel categoryModel)
        {
            var allBugets = BugetBL.GetBugetByCategory(categoryModel.Category);
            this.AllTransactions = this.DataService.GetAllTransactions().ToList();
            var existedtransactions = AllTransactions.Where(t => t.Category.ID == categoryModel.Category.ID).ToList();
            if ((existedtransactions != null && existedtransactions.Count > 0) ||  (allBugets != null && allBugets.Count > 0))
            {
                this.ShowConflictWithTransactionDialog(existedtransactions, allBugets,categoryModel);
                return;
            }

            Action removeCallBack = () =>
                {
                    var deleteCategories = this.Categories.Where(cat => cat.ParentID == categoryModel.Category.ID).ToList();
                    if (deleteCategories.Count() > 0)
                    {
                        CategoryBL.DeleteCategories(deleteCategories.Select(cat => cat.ID).ToList());
                    }
                    this.DeleteCategory(categoryModel);
                    this.Categories.Remove(categoryModel.Category);
                };

            var dialogArgs2 = new Dictionary<string, object>()
                {
                    { "Title",  string.Format("מחיקת קטגוריה {0}",categoryModel.Name) },
                    { "Content", "האם אתה בטוח שברצונך למחוק את הקטגוריה ?" },
                     {"CallBack", removeCallBack }
                };

            await this.DialogService.ShowDialog(DialogKeys.CONFIRM, dialogArgs2);
        }

        private async void ShowConflictWithTransactionDialog(List<Transaction> existedtransactions,List<Buget> bugets, CategoryModel categoryModel)
        {
            Action removeCallBack = () =>
            {
                //if (existedtransactions != null)
                //{
                //    existedtransactions.ForEach(t => TransactionBL.DeleteTransaction(t.ID));
                //}
                //if(bugets != null)
                //{
                //    bugets.ForEach(b => BugetBL.DeleteBuget(b.ID));
                //}
                //if (categoryModel.Category.ParentID == null)
                //{
                //    var deleteCategories = this.Categories.Where(cat => cat.ParentID == categoryModel.Category.ID);
                //    if (deleteCategories.Count() > 0)
                //    {
                //        CategoryBL.DeleteCategories(deleteCategories.Select(cat => cat.ID).ToList());
                //    }
                //}
                this.DeleteCategory(categoryModel);
                this.Categories.Remove(categoryModel.Category);

            };

            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Title", "   קונפליקט עם תנועות" },
                    { "Content", "קיימים תנועות עם קטגוריה זו. האם למחוק את התנועות?" },
                    {"CallBack", removeCallBack }
                };

            await this.DialogService.ShowDialog(DialogKeys.CONFIRM, dialogArgs);

        }

        private void DeleteCategory(CategoryModel categoryModel)
        {
            CategoryGroup group;
            if (categoryModel.Category.TypeID == (int)(Types.Income))
            {
                group = this.IncomesGroups.FirstOrDefault(g => g.CategoryModel.Category.ID == categoryModel.Category.ID);
                if (group != null)
                {
                    this.IncomesGroups.Remove(group);
                }
                else
                {
                    group = this.IncomesGroups.FirstOrDefault(g => g.CategoryModel.Category.ID == categoryModel.Category.ParentID);
                    group.Categories.Remove(categoryModel);
                }

            }
            else
            {
                group = this.ExpensesGroups.FirstOrDefault(g => g.CategoryModel.Category.ID == categoryModel.Category.ID);
                if (group != null)
                {
                    this.ExpensesGroups.Remove(group);
                }
                else
                {
                    group = this.ExpensesGroups.FirstOrDefault(g => g.CategoryModel.Category.ID == categoryModel.Category.ParentID);
                    group.Categories.Remove(categoryModel);
                }
            }

            CategoryBL.DeleteCategory(categoryModel.Category.ID);
            this.RemoveCallBack(categoryModel.Category);
        }

        private void OnAddCategoryCommand()
        {
            Action<Category> callback = category =>
            {
                if (category.TypeID == (int)Types.Income)
                {
                    this.AddCategory(category, this.IncomesGroups);
                }
                else
                {
                    this.AddCategory(category, this.ExpensesGroups);
                }

            };
            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", callback },

                };
            this.DialogService.ShowDialog(DialogKeys.ADD_CATEGORY, dialogArgs);
        }

        private void AddCategory(Category category, ObservableCollection<CategoryGroup> groups)
        {
            CategoryGroup categoryGroup = new CategoryGroup();
            if (category.IsParent)
            {
                categoryGroup.CategoryModel = new CategoryModel();
                categoryGroup.CategoryModel.Category = category;
                categoryGroup.CategoryModel.Name = category.Name;
                categoryGroup.CategoryModel.PictureName = category.PictureName;
                groups.Add(categoryGroup);
                var result2 = CategoryBL.CreateNewCategory(category);
                if (result2.Item1)
                {
                    this.AddCallBack(result2.Item2);
                    return;
                }
            }
            var group = groups.FirstOrDefault(g => g.CategoryModel.Category.ID == category.ParentID);
            if (group == null)
                return;

            if (group.Categories == null)
            {
                group.Categories = new ObservableCollection<CategoryModel>();
            }

            var result = CategoryBL.CreateNewCategory(category);
            if (result.Item1)
            {
                this.AddCallBack(result.Item2);
                group.Categories.Add(new CategoryModel() { Category = result.Item2 ,Name = result.Item2.Name,PictureName = result.Item2.PictureName });
                this.Categories.Add(result.Item2);
                this.AddCallBack(category);
            }
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
            this.AddCategoryCommand = new RelayCommand(OnAddCategoryCommand);
            this.RemoveCommand = new RelayCommand<CategoryModel>(OnRemoveCommand);
            this.UpdateCategoryCommand = new RelayCommand<CategoryModel>(OnUpdateCategoryCommand);
          //  this.UpdateGroupCategoryCommand = new RelayCommand<Category>(OnUpdateGroupCategoryCommand);
        }

        private void SetGroups()
        {
            Categories = this.DataService.GetAllCategories();
            this.IncomesGroups = new ObservableCollection<CategoryGroup>();
            this.ExpensesGroups = new ObservableCollection<CategoryGroup>();
            foreach (var category in Categories)
            {
                if (!category.IsParent)
                    continue;

                CategoryGroup categoryGroup = new CategoryGroup();
                categoryGroup.CategoryModel = new CategoryModel() { Category = category , Name = category.Name , PictureName = category.PictureName };
                var childCategory = Categories.Where(cat => cat.IsParent == false && cat.ParentID == category.ID).ToList();
                categoryGroup.Categories = 
                    new ObservableCollection<CategoryModel>(childCategory.Select(c => new CategoryModel() { Category = c, PictureName = c.PictureName, Name = c.Name }));

                if (category.TypeID == (int)Types.Income)
                {
                    this.IncomesGroups.Add(categoryGroup);
                }
                else
                {
                    this.ExpensesGroups.Add(categoryGroup);
                }
            }
        }

        private void OnUpdateCategoryCommand(CategoryModel categoryModel)
        {
            this.UpdateCategory(categoryModel, false);
        }

        //private void OnUpdateGroupCategoryCommand(Category category)
        //{
        //    this.UpdateCategory(category, true);
        //}

        private void UpdateCategory(CategoryModel categoryModel, bool isGroup)
        {
            Action<Category> callback = cat =>
            {
                CategoryBL.UpdateCategory(cat);
                categoryModel.Name = cat.Name;
                categoryModel.PictureName = cat.PictureName;
                //if (cat.TypeID == (int)Types.Income)
                //{
                //    this.ChangeUpdatedCategory(this.IncomesGroups,cat, isGroup);
                //}
                //else
                //{
                //    this.ChangeUpdatedCategory(this.ExpensesGroups, cat, isGroup);
                //}
            };
            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", callback },
                    {"Category",categoryModel.Category }

                };
            this.DialogService.ShowDialog(DialogKeys.ADD_CATEGORY, dialogArgs);
        }

        //private void ChangeUpdatedCategory(ObservableCollection<CategoryGroup> groups, Category category, bool isGroup)
        //{
        //    if (isGroup)
        //    {
        //        var group = groups.FirstOrDefault(g => g.Category.ID == category.ID);
        //        group.Category = category;
        //    }
        //    else
        //    {
        //        foreach (var group in groups)
        //        {
        //            var updatedCategory = group.Categories.FirstOrDefault(c => c.ID == category.ID);
        //            if (updatedCategory != null)
        //            {
        //                updatedCategory = category;
        //            }
        //        }
        //    }
        //}

        #endregion

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.AddCallBack = args["AddCallBack"] as Action<Category>;
                this.RemoveCallBack = args["RemoveCallBack"] as Action<Category>;
                this.SetGroups();
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }

        #endregion
    }
}
