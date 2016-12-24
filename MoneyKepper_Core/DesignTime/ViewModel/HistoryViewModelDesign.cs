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
    class HistoryViewModelDesign : IHistoryViewModel
    {
        #region Bindable Members

        private ObservableCollection<Category> _categotries;
        public ObservableCollection<Category> Categotries
        {
            get { return _categotries; }
            set { _categotries = value; }
        }

        #endregion

        #region Ctor's
        public HistoryViewModelDesign()
        {
            this.GetCategortiesTransactionsByDate();
        }

        private void GetCategortiesTransactionsByDate()
        {
            Categotries = new ObservableCollection<Category>(CacheManager.Instance.Categories.Values.ToList());
        }

        #endregion

        #region Methods

        #endregion
    }
}
