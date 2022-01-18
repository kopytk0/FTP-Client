using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicClient
{
    public class SampleFileSystemViewModel 
    {
        public string CurrentFolder { get; set; }
        public List<IFileSystemItem> Items { get; set; }

        public SampleFileSystemViewModel()
        {
            CurrentFolder = "\\test";
            Items = new List<IFileSystemItem>()
            {
                new FolderViewModel("...", "\\"),
                new FolderViewModel("\\koty"), new FolderViewModel("\\policja"),
                new FileViewModel("\\File1File2"), new FileViewModel("\\kopytka"),
                new FileViewModel("\\Niedzielski")
            };
        }
        
    }
}
