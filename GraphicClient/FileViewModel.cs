using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTP;

namespace GraphicClient
{
    public class FileViewModel : IFileSystemItem
    {
        public string Name { get; internal set; }

        public string FullPath { get; private set; }

        public FileViewModel(FtpEntry entry)
        {
            Name = entry.Name;
            FullPath = entry.FullPath;
        }

        public FileViewModel(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
        public FileViewModel(string fullPath)
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
