using Models;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepperCore.Service
{
    public interface IActionsService
    {
        void ShowTransactionsDetails(Action<TransactionItem> addCallBack, Action<TransactionItem> removeCallBack, DateTime currentMonth);
        void ShowHistoryGraphs(DateTime startDateTime, DateTime endDateTime, List<Category> Categories, Graph graphType);
        void ShowMonthGraphs(DateTime month);
        void ShowReportDetails(DateTime startDateTime, DateTime endDateTime ,string SubTitle ="");
        void ShowEmptyPage();
        void ShowBugetDetails(Action<BugetItem> addCallBack, Action<BugetItem> removeCallBack, DateTime month);
        void ShowCategoriesDetails(Action<Category> addCallBack, Action<Category> removeCallBack);
    }
}
