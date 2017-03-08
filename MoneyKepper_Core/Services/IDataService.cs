using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyKepper_Core.ViewModel;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using MoneyKepper_Core.Models.Groups;

namespace MoneyKepper2.Service
{
    public interface IDataService
    {
        IList<Transaction> GetTransactionsByType(Types transactionType);
        IList<Category> GetAllCategories();
        //IList<CategoryGroupModel> GetAllCategoriesGroup();
        IList<Category> GetCategoriesByTypes(List<Types> type);
        IList<string> GetAvailableImages();
        IList<Transaction> GetTransactionsByDate(DateTime dateTime);
    }
}
