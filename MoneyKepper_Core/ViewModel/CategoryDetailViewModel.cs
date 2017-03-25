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
        public RelayCommand<Category> RemoveCommand { get; private set; }

        public RelayCommand<Category> UpdateCategoryCommand { get; private set; }
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


        private async void OnRemoveCommand(Category category)
        {
            this.AllTransactions = this.DataService.GetAllTransactions().ToList();
            var existedtransactions = AllTransactions.Where(t => t.Category.ID == category.ID).ToList();
            if (existedtransactions != null && existedtransactions.Count > 0)
            {
                this.ShowConflictWithTransactionDialog(existedtransactions, category);
                return;
            }

            Action removeCallBack = () =>
                {
                    var deleteCategories = this.Categories.Where(cat => cat.ParentID == category.ID).ToList();
                    if (deleteCategories.Count() > 0)
                    {
                        CategoryBL.DeleteCategories(deleteCategories.Select(cat => cat.ID).ToList());
                    }
                    this.DeleteCategory(category);
                    this.Categories.Remove(category);
                };

            var dialogArgs2 = new Dictionary<string, object>()
                {
                    { "Title",  string.Format("מחיקת קטגוריה {0}",category.Name) },
                    { "Content", "האם אתה בטוח שברצונך למחוק את הקטגוריה ?" },
                     {"CallBack", removeCallBack }
                };

            await this.DialogService.ShowDialog(DialogKeys.CONFIRM, dialogArgs2);
        }

        private async void ShowConflictWithTransactionDialog(List<Transaction> existedtransactions, Category category)
        {
            Action removeCallBack = () =>
            {
                existedtransactions.ForEach(t => TransactionBL.DeleteTransaction(t.ID));
                if (category.ParentID == null)
                {
                    var deleteCategories = this.Categories.Where(cat => cat.ParentID == category.ID);
                    if (deleteCategories.Count() > 0)
                    {
                        CategoryBL.DeleteCategories(deleteCategories.Select(cat => cat.ID).ToList());
                    }
                }
                this.DeleteCategory(category);
                this.Categories.Remove(category);

            };
            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Title", "קונפליקט עם תנועות" },
                    { "Content", "קיימים תנועות עם קטגוריה זו. האם למחוק את התנועות?" },
                    {"CallBack", removeCallBack }
                };

            await this.DialogService.ShowDialog(DialogKeys.CONFIRM, dialogArgs);

        }

        private void DeleteCategory(Category category)
        {
            CategoryGroup group;
            if (category.TypeID == (int)(Types.Income))
            {
                group = this.IncomesGroups.FirstOrDefault(g => g.Category.ID == category.ID);
                if (group != null)
                {
                    this.IncomesGroups.Remove(group);
                }
                else
                {
                    group = this.IncomesGroups.FirstOrDefault(g => g.Category.ID == category.ParentID);
                    group.Categories.Remove(category);
                }

            }
            else
            {
                group = this.ExpensesGroups.FirstOrDefault(g => g.Category.ID == category.ID);
                if (group != null)
                {
                    this.ExpensesGroups.Remove(group);
                }
                else
                {
                    group = this.ExpensesGroups.FirstOrDefault(g => g.Category.ID == category.ParentID);
                    group.Categories.Remove(category);
                }
            }

            CategoryBL.DeleteCategory(category.ID);
            this.RemoveCallBack(category);
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
                categoryGroup.Category = category;
                groups.Add(categoryGroup);
                var result2 = CategoryBL.CreateNewCategory(category);
                if (result2.Item1)
                {
                    this.AddCallBack(result2.Item2);
                    return;
                }
            }
            var group = groups.FirstOrDefault(g => g.Category.ID == category.ParentID);
            if (group == null)
                return;

            if (group.Categories == null)
            {
                group.Categories = new ObservableCollection<Category>();
            }
         
            var result = CategoryBL.CreateNewCategory(category);
            if (result.Item1)
            {
                this.AddCallBack(result.Item2);
                group.Categories.Add(result.Item2);
                this.Categories.Add(result.Item2);
                this.AddCallBack(category);
            }
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
            this.AddCategoryCommand = new RelayCommand(OnAddCategoryCommand);
            this.RemoveCommand = new RelayCommand<Category>(OnRemoveCommand);
            this.UpdateCategoryCommand = new RelayCommand<Category>(OnUpdateCategoryCommand);
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
                categoryGroup.Category = category;
                var childCategory = Categories.Where(cat => cat.IsParent == false && cat.ParentID == category.ID).ToList();
                categoryGroup.Categories = new ObservableCollection<Category>(childCategory);

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

        private void OnUpdateCategoryCommand(Category category)
        {
            this.UpdateCategory(category);
        }

        private void UpdateCategory(Category category)
        {
            Action<Category> callback = c =>
            {
                CategoryBL.UpdateCategory(c);
            };
            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", callback },
                    {"Category",category }

                };
            this.DialogService.ShowDialog(DialogKeys.ADD_CATEGORY, dialogArgs);
        }


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
