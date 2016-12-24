using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using Models;
using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.DesignTime.ViewModel
{
    public class TransactionsDetailsViewModelDesign : ITransactionsDetailsViewModel
    {
        #region Members

        #endregion

        #region Bindable Properties

        public string CurrentMonth { get; set; }
        public int Income { get; set; }
        public int Expenses { get; set; }
        public int Balance { get; set; }

        public ObservableCollection<TransactionItem> IncomeItems { get; set; }
        public ObservableCollection<TransactionItem> ExpensesItems { get; set; }

        #endregion

        #region Ctor's
        public TransactionsDetailsViewModelDesign()
        {
            this.CurrentMonth = DateTime.Now.Month.ToString("MM");
            this.Income = 4000;
            this.Expenses = 200;
            this.Balance = 3800;
            this.SetIncomeItemsAndExpensesItems();
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            var categories = CacheManager.Instance.Categories.Values.ToList();
            this.IncomeItems = new ObservableCollection<TransactionItem>();
            this.ExpensesItems = new ObservableCollection<TransactionItem>();
            for (int i = 0; i < 10; i++)
            {
                Transaction transaction = new Transaction()
                {
                    Amount = i + 120,
                    CategoryID = i,
                    Date = DateTime.Now.AddDays(i),
                    Note = "חשוב"
                };
                Transaction transaction2 = new Transaction()
                {
                    Amount = i + 500,
                    CategoryID = i + 11,
                    Date = DateTime.Now.AddDays(i),
                    Note = "הוצאה"
                };
                TransactionItem transactionItem = new TransactionItem(transaction, categories[0]);
                this.IncomeItems.Add(transactionItem);
                TransactionItem transactionItem2 = new TransactionItem(transaction2, categories[1]);
                this.ExpensesItems.Add(transactionItem2);
            }
        }

        #endregion

        #region Commands

        #endregion
    }
}
