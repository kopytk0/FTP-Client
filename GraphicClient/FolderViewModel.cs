using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTP;

namespace GraphicClient
{
    public class FolderViewModel : IFileSystemItem
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }

        public FolderViewModel(FtpEntry entry)
        {
            Name = entry.Name;
            FullPath = entry.FullPath;
        }

        public FolderViewModel(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
        public FolderViewModel(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
