using MoneyKepper_Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Factories
{
    public interface IDialogServiceFactory
    {
        IDialogService GetInstance();
    }
}
