﻿using Models;
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
        void ShowTransactionsDetails(Action<Tuple<TransactionsViewModel.Types, double>> addCallBack , Action<Tuple<TransactionsViewModel.Types, double>> removeCallback);
        void ShowHistoryGraphs(DateTime startDateTime, DateTime endDateTime, List<Category> Categories, Graph graphType);
        void ShowMonthGraphs(DateTime startDateTime, DateTime endDateTime, Graph graphType);
        void ShowEmptyPage();
    }
}