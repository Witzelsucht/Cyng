using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Veritaware.Toolkits.LightVM.Std;

namespace Cyng.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private ObservableCollection<string> _results;
        public ObservableCollection<string> Results
        {
            get { return _results; }
            set { Set(ref _results, value); }
        }

        private bool _wasFound;
        public bool WasFound
        {
            get { return _wasFound; }
            set { Set(ref _wasFound, value); }
        }

        private string _foundText;
        public string FoundText
        {
            get { return _foundText; }
            set { Set(ref _foundText, value); }
        }

        private string _selectedWebsite;
        public string SelectedWebsite
        {
            get { return _selectedWebsite; }
            set { Set(ref _selectedWebsite, value);
                  OpenWebsite(_selectedWebsite); }
        }


        public RelayCommand BackCommand { get; private set; }

        private MainWindowViewModel _mwvm;
        public SearchViewModel()
        {
            var loc = ViewModelLocator.Get(typeof(MainWindowViewModel));
            if (loc is MainWindowViewModel MWVM)
            {
                _mwvm = MWVM;
            }
            BackCommand = new RelayCommand(Back);
        }

        private void OpenWebsite(string website)
        {
            if(website != null)
            {
                Process.Start(website);
            }
        }
        
        public void Search()
        {
            try
            {
                Results = new ObservableCollection<string>();
                WasFound = false;
                FoundText = "Nie znaleziono żadnych wyników";
                foreach (string website in _mwvm.Websites)
                {
                    using (WebClient client = new WebClient())
                    {
                        string htmlCode = client.DownloadString(website);
                        htmlCode = htmlCode.ToLower();
                        foreach (string keyword in _mwvm.Keywords)
                        {
                            if (htmlCode.Contains(keyword.ToLower()))
                            {
                                if (!Results.Contains(website))
                                {
                                    App.Current.Dispatcher.Invoke((Action)delegate
                                    {
                                        Results.Add(website);
                                    });
                                    WasFound = true;
                                }
                            }
                        }
                    }
                }
                if(WasFound)
                {
                    FoundText = $"Znaleziono {Results.Count} stron z podanymi słowami";
                }
                else
                {
                    FoundText = "Nie znaleziono żadnych wyników";
                }
            }
            catch (Exception ex)
            {
                Back();
                _mwvm.ShowMessage("Błąd", "Wystąpił błąd przy przeszukiwaniu stron. \n Sprawdź podane strony");         
            }
        }
        private void Back()
        {
            _mwvm.VisSearch = false;
            _mwvm.VisMenu = true;
        }
    }
}
