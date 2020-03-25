using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veritaware.Toolkits.LightVM.Std;

namespace Cyng.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private MainWindowViewModel _mwvm;
        private SearchViewModel _svm;
        public RelayCommand ScanCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }

        private bool _visLoading;

        public bool VisLoading
        {
            get { return _visLoading; }
            set { Set(ref _visLoading, value); }
        }


        public MainMenuViewModel()
        {
            
            LoadCommands();
            LoadLocators();
        }

        private void LoadLocators()
        {
            var loc = ViewModelLocator.Get(typeof(MainWindowViewModel));
            if (loc is MainWindowViewModel MWVM)
            {
                _mwvm = MWVM;
            }
            var loc2 = ViewModelLocator.Get(typeof(SearchViewModel));
            if (loc2 is SearchViewModel SVM)
            {
                _svm = SVM;
            }
        }

        private void LoadCommands()
        {
            ScanCommand = new RelayCommand(ScanAsync);
            SettingsCommand = new RelayCommand(Settings);
        }

        private async void ScanAsync()
        {
            VisLoading = true;
            await Task.Run(() => { _svm.Search(); });
            VisLoading = false;
            _mwvm.VisMenu = false;
            _mwvm.VisSearch = true;
        }

        private void Settings()
        {
            _mwvm.VisMenu = false;
            _mwvm.VisOptions = true;
        }
    }
}
