using MoneyKepper_Core.Factories;
using MoneyKepper_Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper2.Factories
{
    public class DialogServiceFactory : IDialogServiceFactory
    {

        private IDialogService DialogService { get; set; }

        public IDialogService GetInstance()
        {
            if (this.DialogService == null)
            {
                this.DialogService = new DialogService();
            }

            return this.DialogService;
        }
    }

}
