
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.Managers
{
    /// <summary>
    /// Singleton class the give encapsulate the access to application data cache
    /// </summary>
    class CacheManager : ICacheManager
    {
        #region Static Members

        static private object _instanceLocker = new object();
        static private CacheManager _instance;
        static public CacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLocker)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Public Dictionaries

        public Dictionary<int, Category> Categories { get; private set; }
        public List<string> ImagesName { get; private set; }

        #endregion

        #region Constructors

        private CacheManager() { }

        #endregion

        #region public Methods

        public void LoadAll()
        {
            LoadCategories();
            LoadImagesName();
        }

        private void LoadImagesName()
        {
            ImagesName = new List<string>();
            for (int i = 1; i <= 75; i++)
            {
                ImagesName.Add(i.ToString());
            }
        }

        private void LoadCategories()
        {
            this.Categories = new Dictionary<int, Category>();
            this.Categories.Add(1, new Category( "מסעדות", (int)Types.Expenses, "23", false, 15));
            this.Categories.Add(2, new Category("מצרכים", (int)Types.Expenses, "67", false, 15));
            this.Categories.Add(3, new Category("משכנתא", (int)Types.Expenses, "50", false, 16));
            this.Categories.Add(4, new Category("טלפון", (int)Types.Expenses, "63", false, 16));
            this.Categories.Add(5, new Category("חשמל", (int)Types.Expenses, "68", false, 16));
            this.Categories.Add(6, new Category("מים", (int)Types.Expenses, "73", false, 16));
            this.Categories.Add(7, new Category("משכורת", (int)Types.Income, "48", false, 17));
            this.Categories.Add(8, new Category("הלבשה אישית", (int)Types.Expenses, "16", false, 18));
            this.Categories.Add(9, new Category("נעלים", (int)Types.Expenses, "65", false, 18));
            this.Categories.Add(10, new Category("בגדי ילדים", (int)Types.Expenses, "14", false, 18));
            this.Categories.Add(11, new Category("ביטוח בריאות", (int)Types.Expenses, "17", false, 19));
            this.Categories.Add(12, new Category("תרופות", (int)Types.Expenses, "18", false, 19));
            this.Categories.Add(13, new Category("דלק", (int)Types.Expenses, "29", false, 20));
            this.Categories.Add(14, new Category("ביטוח רכב", (int)Types.Expenses, "9", false, 20));
            this.Categories.Add(15, new Category("אוכל", (int)Types.Expenses, "22", true));
            this.Categories.Add(16, new Category("דיור", (int)Types.Expenses, "40", true));
            this.Categories.Add(17, new Category("הכנסות", (int)Types.Income, "47", true));
            this.Categories.Add(18, new Category("ביגוד", (int)Types.Expenses, "15", true));
            this.Categories.Add(19, new Category("רפואה", (int)Types.Expenses, "19", true));
            this.Categories.Add(20, new Category("תחבורה", (int)Types.Expenses, "11", true));
        }
        #endregion
    }
}
