using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FTP;

namespace GraphicClient
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public ConfigModel Config { get; set; }
        public ICommand Connect { get; set; }

        public FtpFileSystemViewModel FileSystem { get; private set; }

        public bool Save { get; set; }
        internal LoginViewModel(string configPath)
        {
            Config = ConfigModel.LoadConfig(Consts.ConfigPath);
            FileSystem = new FtpFileSystemViewModel();

            Config ??= new ConfigModel();

            Connect = new Command((o =>
            {
                if (Save)
                {
                    Config.SaveConfig(Consts.ConfigPath);
                }

                FileSystem = new FtpFileSystemViewModel(Config.Username, Config.Password, new Client(Config.Ip));
                NotifyPropertyChanged(nameof(FileSystem));

            }));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
