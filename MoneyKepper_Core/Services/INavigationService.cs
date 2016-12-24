using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepperCore.ViewModel.ControlPanelViewModel;

namespace MoneyKepperCore.Service
{
    public interface INavigationService
    {
        ICoreNavigationService PrimaryFrame { get; }
        ICoreNavigationService DetailsFrame { get; }


    }
 
}
