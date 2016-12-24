using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public abstract class DialogViewModel : ViewModelCore, IDialog
    {
        public Action Hide
        {
            get; private set;
        }

        public IDialog Dialog
        {
            get { return this; }
        }

        public void Initialize(Action hideAction)
        {
            this.Hide = hideAction;
        }

        public abstract void OnShow(object parameter);
    }
}
