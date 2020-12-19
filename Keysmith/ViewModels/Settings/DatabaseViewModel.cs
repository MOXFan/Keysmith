using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin;

namespace Keysmith.ViewModels
{
    class DatabaseViewModel
    {
        #region Constructors
        public DatabaseViewModel()
        {
            LoadDBCommand = new Command(LoadDB);
            SaveDBCommand = new Command(SaveDB);
        }
        #endregion
        #region Properties
        public ICommand LoadDBCommand { get; private set; }
        public ICommand SaveDBCommand { get; private set; }
        #endregion
        #region Methods
        private void LoadDB()
        {
            Plugin.FilePicker.CrossFilePicker.Current.PickFile();
        }
        private void SaveDB()
        { }
        #endregion
    }
}
