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

namespace MoneyKepper2.Service
{
    public class DataService : IDataService
    {
        public IList<Transaction> GetTransactionsByType(Types transactionType)
        {
            List<Transaction> Transactions = new List<Transaction>();
            switch (transactionType)
            {
                case Types.Income:
                    Transaction transaction = new Transaction()
                    {
                        Amount = 8000,
                        CategoryID = 1,
                        Date = DateTime.Now.AddDays(5),
                        Note = "חשוב"
                    };
                    Transaction transaction1 = new Transaction()
                    {
                        Amount = 4000,
                        CategoryID = 1,
                        Date = DateTime.Now.AddDays(6),
                    };
                    Transaction transaction2 = new Transaction()
                    {
                        Amount = 400,
                        CategoryID = 6,
                        Date = DateTime.Now.AddDays(7),
                        Note = "מיוחדות"
                    }; Transaction transaction3 = new Transaction()
                    {
                        Amount = 80,
                        CategoryID = 6,
                        Date = DateTime.Now.AddDays(1),
                    };
                    Transactions.Add(transaction);
                    Transactions.Add(transaction1);
                    Transactions.Add(transaction2);
                    Transactions.Add(transaction3);
                    break;

                case Types.Expenses:
                    Transaction transaction4 = new Transaction()
                    {
                        Amount = 2500,
                        CategoryID = 2,
                        Date = DateTime.Now.AddDays(1),
                    };
                    Transaction transaction5 = new Transaction()
                    {
                        Amount = 120,
                        CategoryID = 4,
                        Date = DateTime.Now.AddDays(1),
                    };
                    Transaction transaction6 = new Transaction()
                    {
                        Amount = 400,
                        CategoryID = 4,
                        Date = DateTime.Now.AddDays(1),
                    }; Transaction transaction7 = new Transaction()
                    {
                        Amount = 600,
                        CategoryID = 3,
                        Date = DateTime.Now.AddDays(1),
                        Note = "ילדים"
                    };
                    Transactions.Add(transaction4);
                    Transactions.Add(transaction5);
                    Transactions.Add(transaction6);
                    Transactions.Add(transaction7);
                    break;
            }
            return Transactions;
        }

        public IList<Category> GetAllCategories()
        {
            return CacheManager.Instance.Categories.Values.Where(cat=>cat.IsActive).ToList();
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
            List<Category> categories = new List<Category>();
            var allCategories = CacheManager.Instance.Categories.Values.ToList();
            foreach (var type in types)
            {
                var categoryType = allCategories.Where(cat => cat.TypeID == (int)type);
                categories.AddRange(categoryType);
            }

            return categories;
        }

        public IList<string> GetAvailableImages()
        {
            var imagesName = CacheManager.Instance.Categories.Values.Select(cat => cat.PictureName).ToList();
            var allImages = CacheManager.Instance.ImagesName;
            return allImages.Except(imagesName).ToList();
        }
    }
}
