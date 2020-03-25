using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Veritaware.Toolkits.LightVM.Std;
using System.Collections.ObjectModel;

namespace Cyng.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> Websites = new ObservableCollection<string>();
        public ObservableCollection<string> Keywords = new ObservableCollection<string>();
        public RelayCommand BackCommand { get; private set; }

        private string _messageType;
        public string MessageType
        {
            get { return _messageType; }
            set { Set(ref _messageType, value); }
        }

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set { Set(ref _messageText, value); }
        }

        private bool _visMenu;
        public bool VisMenu
        {
            get { return _visMenu; }
            set { Set(ref _visMenu, value); }
        }

        private bool _visSearch;
        public bool VisSearch
        {
            get { return _visSearch; }
            set { Set(ref _visSearch, value); }
        }

        private bool _visOptions;
        public bool VisOptions
        {
            get { return _visOptions; }
            set { Set(ref _visOptions, value); }
        }

        private bool _visMessage;
        public bool VisMessage
        {
            get { return _visMessage; }
            set { Set(ref _visMessage, value); }
        }

        public MainWindowViewModel()
        {
            VisMenu = true;
            BackCommand = new RelayCommand(Back);
            LoadWebsites();
            LoadKeywords();
        }

        public void LoadWebsites(string path = "default_websites.txt")
        {
            if(File.Exists(path))
            {
                using(StreamReader file = new StreamReader(path))
                {
                    string line;
                    while((line = file.ReadLine())!= null)
                    {
                        Websites.Add(line);
                    }
                }
            }
            else
            {
                File.Create(path);
            }
        }

        public void LoadKeywords(string path = "default_keywords.txt")
        {
            if (File.Exists(path))
            {
                using (StreamReader file = new StreamReader(path))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        Keywords.Add(line);
                    }
                }
            }
            else
            {
                File.Create(path);
            }
        }

        public void ShowMessage(string messageType, string messageText)
        {
            MessageText = messageText;
            MessageType = messageType;
            VisMessage = true;
        }

        private void Back()
        {
            VisMessage = false;
        }
    }
}
