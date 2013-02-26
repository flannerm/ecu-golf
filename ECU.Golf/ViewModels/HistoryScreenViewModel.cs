using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.DataAccess;

namespace ECU.Golf.ViewModels
{
    public class HistoryScreenViewModel : ViewModelBase
    {
        #region Private Members

        private Dictionary<Int16, string> _historyCategories;

        private List<string> _selectedCategoryItems;

        private KeyValuePair<Int16, string> _selectedCategory;

        #endregion

        #region Properties

        public Dictionary<Int16, string> HistoryCategories
        {
            get { return _historyCategories; }
            set { _historyCategories = value; OnPropertyChanged("HistoryCategories"); }
        }

        public List<string> SelectedCategoryItems
        {
            get { return _selectedCategoryItems; }
            set { _selectedCategoryItems = value; OnPropertyChanged("SelectedCategoryItems"); }
        }

        public KeyValuePair<Int16, string> SelectedCategory
        {
            get { return _selectedCategory; }
            set 
            { 
                _selectedCategory = value; 
                OnPropertyChanged("SelectedCategory");

                SelectedCategoryItems = DbConnection.GetHistoryItems(_selectedCategory.Key);
            }
        }

        #endregion

        #region Constructor

        public HistoryScreenViewModel()
        {
            _historyCategories = DbConnection.GetHistoryCategories();
        }

        #endregion
    }
}
