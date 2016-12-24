using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public interface IDialog
    {
        Action Hide { get; }
        void Initialize(Action hideAction);
        void OnShow(object parameter);
    }
}
