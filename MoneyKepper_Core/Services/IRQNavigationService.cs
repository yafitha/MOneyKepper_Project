using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepperCore.ViewModel.ControlPanelViewModel;

namespace MoneyKepper_Core.Services
{
    public interface IRQNavigationService
    {
        Dictionary<string, object> NavigationParameter { get; }

        MoneyKepperCore.ViewModel.ControlPanelViewModel.PrimaryPanel SelectedPrimaryPanel { get; }

        void NavigateToDefaultView();
        void NavigateTo(PrimaryPanel key, Dictionary<string, object> paramter = null);
        void GoBack();
    }
}
