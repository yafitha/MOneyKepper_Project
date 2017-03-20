using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.ViewModel;
using MoneyKepper_Core.Models.Groups;
using MoneyKepper_Core.BL;

namespace MoneyKepper2.Service
{
    public class DataService : IDataService
    {
        public IList<Transaction> GetTransactionsByType(Types transactionType)
        {
            var types = new List<int>();
            types.Add((int)transactionType);
            return TransactionBL.GetTransactionsByTypes(types);
        }

        public IList<Category> GetAllCategories()
        {
            // return CacheManager.Instance.Categories.Values.Where(cat => cat.IsActive).ToList();
            return CategoryBL.GetAllCategories();
        }

        //public IList<CategoryGroupModel> GetAllCategoriesGroup()
        //{
        //    List<CategoryGroupModel> categoryGroupModels = new List<CategoryGroupModel>();
        //    var allCategories = CacheManager.Instance.Categories.Values.ToList();
        //    var categoryGroups = CacheManager.Instance.CategoryGroups.Values.ToList();
        //    foreach (var categoryGroup in categoryGroups)
        //    {
        //        var categories = allCategories.Where(c => c.CategoryID == categoryGroup.ID);
        //        CategoryGroupModel categoryGroupModel = new CategoryGroupModel();
        //        categoryGroupModel.CategoryGroup = categoryGroup;
        //        categoryGroupModel.Categories = categories != null ? new System.Collections.ObjectModel.ObservableCollection<Category>(categories) : null;
        //        categoryGroupModels.Add(categoryGroupModel);
        //    }
        //    return categoryGroupModels;
        //}

        public IList<Category> GetCategoriesByTypes(List<Types> types)
        {
            return CategoryBL.GetCategoriesByTypes(types.Select(t => (int)t).ToList());
        }

        public IList<string> GetAvailableImages()
        {
            var imagesName = this.GetAllCategories().Select(cat => cat.PictureName).ToList();
            var allImages = CacheManager.Instance.ImagesName;
            return allImages.Except(imagesName).ToList();
        }

        public IList<Transaction> GetTransactionsByDate(DateTime dateTime)
        {
            return TransactionBL.GetTransactionsByDate(dateTime);
        }

        public IList<Transaction> GetTransactionsByDateAndType(DateTime startDateTime, DateTime endDateTime, int? typeID)
        {
            return TransactionBL.GetTransactionsByDatesAndType(startDateTime, endDateTime, typeID);
        }

        public IList<Transaction> GetAllTransactions()
        {
            return TransactionBL.GetAllTransactions();
        }
    }
}
