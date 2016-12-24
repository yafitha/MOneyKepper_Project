
using MoneyKepper_Core.Managers;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepperCore.ViewModel
{
    public class MainViewModel : ViewModelCore
    {
        #region MEmbers

        #endregion

        #region Cont's
        public MainViewModel()
        {
            CacheManager.Instance.LoadAll();
        }
        #endregion

    }
}
