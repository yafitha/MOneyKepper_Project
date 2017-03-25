using Models;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.DesignTime.ViewModel
{
    public class BugetDetailsViewModelDesign : IBugetDetailsViewModel
    {
        public ObservableCollection<BugetItem> IncomeItems { get; set; }
        public ObservableCollection<BugetItem> ExpensesItems { get; set; }
        public ObservableCollection<TransactionItem> CategoryTransactions { get; set; }
        public BugetDetailsViewModelDesign()
        {
            SetIncomeItemsAndExpensesItems();
        }

        private void SetIncomeItemsAndExpensesItems()
        {
           var categories = new Dictionary<int, Category>();
            categories.Add(1, new Category("מסעדות", (int)Types.Expenses, "23", false, 15));
            categories.Add(2, new Category("מצרכים", (int)Types.Expenses, "67", false, 15));
            categories.Add(3, new Category("משכנתא", (int)Types.Expenses, "50", false, 16));
            categories.Add(4, new Category("טלפון", (int)Types.Expenses, "63", false, 16));
            categories.Add(5, new Category("חשמל", (int)Types.Expenses, "68", false, 16));
            categories.Add(6, new Category("מים", (int)Types.Expenses, "73", false, 16));
            this.IncomeItems = new ObservableCollection<BugetItem>();
            this.ExpensesItems = new ObservableCollection<BugetItem>();
            this.CategoryTransactions = new ObservableCollection<TransactionItem>();
            for (int i = 1; i < 6; i++)
            {
                Buget buget = new Buget()
                {
                    Amount = i + 120,
                    Category = categories[i],
                    Date = DateTime.Now.AddDays(i),
                    Note = "חשוב"
                };

                Buget buget2 = new Buget()
                {
                    Amount = i + 120,
                    Category = categories[i],
                    Date = DateTime.Now.AddDays(i),
                    Note = "חשוב"
                };

                //Transaction transaction = new Transaction()
                //{
                //    Amount = i + 500,
                //    CategoryID = i,
                //    Date = DateTime.Now.AddDays(i),
                //    Note = "הוצאה"
                //};
                //TransactionItem transactionItem = new TransactionItem(transaction, categories[0]);
                //this.CategoryTransactions.Add(transactionItem);
                BugetItem bugetItem = new BugetItem(buget, categories[i]);
                this.IncomeItems.Add(bugetItem);
                BugetItem bugetItem2 = new BugetItem(buget2, categories[i]);
                this.ExpensesItems.Add(bugetItem2);
            }
        }
    }
}
