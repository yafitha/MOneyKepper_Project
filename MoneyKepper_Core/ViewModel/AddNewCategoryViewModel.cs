using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using Models;
using MoneyKepper2.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public class AddNewCategoryViewModel : DialogViewModel, IAddNewCategoryViewModel
    {
        private List<string> _images;
        public List<string> Images
        {
            get { return _images; }
            set { this.Set(ref _images, value); }
        }
        #region Commands

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        #endregion
        public IDataService DataService { get; set; }
        public Action<Category> CallBack { get; private set; }

        public AddNewCategoryViewModel(IDataService dataService)
        {
            this.DataService = dataService;
        }
        public override void OnShow(object parameter)
        {
            this.CallBack = (parameter as Dictionary<string, object>)["Callback"] as Action<Category>;
            SetImages();
            this.SetCommands();
        }

        private void SetCommands()
        {
            this.CloseCommand = new RelayCommand(() => this.Hide());
           // this.SaveCommand = new RelayCommand(()=>this.CallBack(new Category(21,)));
        }

        private void SetImages()
        {
            this.Images = new List<string>(this.DataService.GetAvailableImages());
        }
    }
}
