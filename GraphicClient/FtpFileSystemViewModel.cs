using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FTP;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace GraphicClient
{
    internal class FtpFileSystemViewModel : INotifyPropertyChanged
    {
        public string CurrentFolderPath { get; set; }
        public ObservableCollection<IFileSystemItem> Items { get; set; }
        private Client ftpClient;
        public ICommand OpenFolder { get; private set; }
        public ICommand DownloadFiles { get; private set; }
        public ICommand UploadFileCommand { get; private set; }
        private byte? _currentProgress;

        public event PropertyChangedEventHandler? PropertyChanged;

        public byte? CurrentProgress
        {
            get
            {
                return _currentProgress;
            }
            set
            {
                if (value == _currentProgress)
                {
                    return;
                }
                _currentProgress = value;
                RisePropertyChanged();
            }
        }

        public void RisePropertyChanged([CallerMemberName]string? name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        internal FtpFileSystemViewModel(string login, SecureString password, Client ftpClient)
        {
            this.ftpClient = ftpClient;
            Items = new ObservableCollection<IFileSystemItem>();
            CurrentFolderPath = @"";
            OpenFolder = new Command(o =>
            {
                try
                {
                    this.GenerateFileList(o.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            });

            DownloadFiles = new Command((o =>
            {
               VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
               var result = dialog.ShowDialog();
               if (result != true)
               {
                   return;
               }
               foreach (IFileSystemItem item in (IList)o)
               {
                   string targetPath = Path.Combine(dialog.SelectedPath, item.Name);
                   if (item is FileViewModel)
                   {
                       this.ftpClient.GetFile(item.FullPath, targetPath);
                   }
                   else if (item is FolderViewModel)
                   {
                       DownloadFolder(item.FullPath, targetPath);
                   }
               }
            }));

            UploadFileCommand = new Command((async o =>
            {
                var dialog = new OpenFileDialog();
                var result = dialog.ShowDialog();
                if (result != true)
                {
                    return;
                }

                CurrentProgress = 0;
                //var progressReporter = new Action<byte>();
                await this.UploadFileAsync(Path.Combine(CurrentFolderPath, dialog.SafeFileName), dialog.FileName,
                    progress => this.CurrentProgress = progress);
                this.OpenFolder.Execute(CurrentFolderPath);
                CurrentProgress = null;
            }));

            ftpClient.Login(login, password);
            OpenFolder.Execute(CurrentFolderPath);
        }

        private void DownloadFolder(string ftpPath, string targetPath)
        {
            System.IO.Directory.CreateDirectory(targetPath);
            var entries = ftpClient.ListFiles(ftpPath);
            foreach (var entry in entries)
            {
                if (entry.Type == FtpEntry.EntryType.File)
                {
                    this.ftpClient.GetFile(entry.FullPath, Path.Combine(targetPath, entry.Name));
                }
                else if (entry.Type == FtpEntry.EntryType.Directory)
                {
                    DownloadFolder(entry.FullPath, Path.Combine(targetPath, entry.Name));
                }
            }
        }

        internal void UploadFile(string serverPath, string localPath)
        {
            ftpClient.UploadFile(localPath, serverPath);
        }
        internal async Task UploadFileAsync(string serverPath, string localPath, Action<byte> progressReporter = null)
        {
            await ftpClient.UploadFileAsync(localPath, serverPath, progressReporter).ConfigureAwait(false);
        }
        internal FtpFileSystemViewModel()
        {
            Items = new ObservableCollection<IFileSystemItem>();
        }

        internal void GenerateFileList(string folderPath)
        {
            Items.Clear();
            
            CurrentFolderPath = folderPath;
            string parent = PathHelper.GetParent(CurrentFolderPath);
            Items.Add(new FolderViewModel(Consts.ParentName, parent == null ? @"" : parent));
            
            var entries = ftpClient.ListFiles(CurrentFolderPath);
            foreach (var item in entries)
            {
                Items.Add(item.Type == FtpEntry.EntryType.Directory ? new FolderViewModel(item) : new FileViewModel(item));
            }
        }
        
    }
}
