using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Veritaware.Toolkits.LightVM.Std;

namespace Cyng.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        public RelayCommand BackCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand AddWebsiteCommand { get; private set; }
        public RelayCommand RemoveWebsiteCommand { get; private set; }
        public RelayCommand ImportWebsitesCommand { get; private set; }
        public RelayCommand ExportWebsitesCommand { get; private set; }

        public RelayCommand AddKeywordCommand { get; private set; }
        public RelayCommand RemoveKeywordCommand { get; private set; }
        public RelayCommand ImportKeywordsCommand { get; private set; }
        public RelayCommand ExportKeywordsCommand { get; private set; }

        private MainWindowViewModel _mwvm;

        private string _websiteText;
        public string WebsiteText
        {
            get { return _websiteText; }
            set { Set(ref _websiteText, value); }
        }

        private string _keywordText;
        public string KeywordText
        {
            get { return _keywordText; }
            set { Set(ref _keywordText, value); }
        }

        private string _selectedWebsite;
        public string SelectedWebsite
        {
            get { return _selectedWebsite; }
            set { Set(ref _selectedWebsite, value); }
        }

        private string _selectedKeyword;
        public string SelectedKeyword
        {
            get { return _selectedKeyword; }
            set { Set(ref _selectedKeyword, value); }
        }

        private ObservableCollection<string> _websitesList;
        public ObservableCollection<string> WebsitesList
        {
            get { return _websitesList; }
            set { Set(ref _websitesList, value); }
        }

        private ObservableCollection<string> _keywordsList;
        public ObservableCollection<string> KeywordsList
        {
            get { return _keywordsList; }
            set { Set(ref _keywordsList, value); }
        }

        public OptionsViewModel()
        {
            var loc = ViewModelLocator.Get(typeof(MainWindowViewModel));
            if (loc is MainWindowViewModel MWVM)
            {
                _mwvm = MWVM;
            }
            LoadCommands();
            ReloadLists();
        }

        private void ReloadLists()
        {
            KeywordsList = _mwvm.Keywords;
            WebsitesList = _mwvm.Websites;
        }

        private void LoadCommands()
        {
            BackCommand = new RelayCommand(Back);
            SaveCommand = new RelayCommand(Save);

            AddWebsiteCommand = new RelayCommand(AddWebsite);
            RemoveWebsiteCommand = new RelayCommand(RemoveWebsite);
            ImportWebsitesCommand = new RelayCommand(ImportWebsites);
            ExportWebsitesCommand = new RelayCommand(ExportWebsites);

            AddKeywordCommand = new RelayCommand(AddKeyword);
            RemoveKeywordCommand = new RelayCommand(RemoveKeyword);
            ImportKeywordsCommand = new RelayCommand(ImportKeywords);
            ExportKeywordsCommand = new RelayCommand(ExportKeywords);
        }

        private void Save()
        {
            using (var writer = new StreamWriter("default_websites.txt"))
            {
                foreach (string website in _mwvm.Websites)
                {
                    writer.WriteLine(website);
                }
            }
            using (var writer = new StreamWriter("default_keywords.txt"))
            {
                foreach (string keyword in _mwvm.Keywords)
                {
                    writer.WriteLine(keyword);
                }
            }
            _mwvm.ShowMessage("Komunikat", "Pomyślnie zapisano nowe domyślne wartości.");
        }

        private void AddWebsite()
        {
            if (WebsiteText != "" && WebsiteText != null)
            {
                _mwvm.Websites.Add(WebsiteText.ToLower());
                WebsiteText = "";
                ReloadLists();
            }
            else
            {
                _mwvm.ShowMessage("Błąd", "Strona internetowa nie może być pusta");
            }
        }

        private void RemoveWebsite()
        {
            if (SelectedWebsite == null)
            {
                _mwvm.Websites.Remove(_mwvm.Websites.Last());
            }
            _mwvm.Websites.Remove(SelectedWebsite);
            ReloadLists();
        }

        private void ImportWebsites()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                string filePath;
                fileDialog.Filter = "Pliki tekstowe (*.txt) |*.txt";
                if (fileDialog.ShowDialog() == DialogResult.OK) ;
                {
                    filePath = fileDialog.FileName;
                    _mwvm.LoadWebsites(filePath);
                }
            }
        }

        private void ExportWebsites()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                string filePath;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = folderDialog.SelectedPath;
                    using (var writer = new StreamWriter(filePath + "\\exported_websites.txt"))
                    {
                        foreach (string keyword in _mwvm.Keywords)
                        {
                            writer.WriteLine(keyword);
                        }
                    }
                    _mwvm.ShowMessage("Komunikat", "Pomyślnie wyeksportowano strony internetowe.");
                }
            }
        }

        private void AddKeyword()
        {
            if(KeywordText != "" && KeywordText != null)
            {
                _mwvm.Keywords.Add(KeywordText.ToLower());
                KeywordText = "";
                ReloadLists();
            }
            else
            {
                _mwvm.ShowMessage("Błąd", "Słowo kluczowe nie może być puste");
            }
        }

        private void RemoveKeyword()
        {
            if(SelectedKeyword == null)
            {
                _mwvm.Keywords.Remove(_mwvm.Keywords.Last());
            }
            _mwvm.Keywords.Remove(SelectedKeyword);
            ReloadLists();
        }

        private void ImportKeywords()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                string filePath;
                fileDialog.Filter = "Pliki tekstowe (*.txt) |*.txt";
                if (fileDialog.ShowDialog() == DialogResult.OK) ;
                {
                    filePath = fileDialog.FileName;
                    _mwvm.LoadKeywords(filePath);
                }
            }
        }

        private void ExportKeywords()
        {
            using(var folderDialog = new FolderBrowserDialog())
            {
                string filePath;
                if(folderDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = folderDialog.SelectedPath;
                    using(var writer = new StreamWriter(filePath+"\\exported_keywords.txt"))
                    {
                        foreach(string keyword in _mwvm.Keywords)
                        {
                            writer.WriteLine(keyword);
                        }
                    }
                    _mwvm.ShowMessage("Komunikat", "Pomyślnie wyeksportowano słowa kluczowe.");
                }
            }
        }

        private void Back()
        {
            _mwvm.VisOptions = false;
            _mwvm.VisMenu = true;
        }

    }
}
