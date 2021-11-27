using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FTP
{
    public class FtpEntry
    {
        public enum EntryType
        {
            File,
            Directory,
            Unknown
        }

        public EntryType Type { get; private set; }
        public DateTime ModifyDate { get; private set; }
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public ulong FileSize { get; private set; }

        internal FtpEntry(EntryType type, DateTime creationDate, string fullPath, ulong fileSize)
        {
            Type = type;
            ModifyDate = creationDate;
            FullPath = fullPath;
            FileSize = fileSize;
            var indyk= FullPath.LastIndexOf('\\');
            Name = indyk < 0 ? FullPath : FullPath.Substring(indyk + 1);
        }

        internal static FtpEntry Parse(string rawData, string parent = "")
        {
            var fileIndex = rawData.IndexOf("; ");
            var fileName = fileIndex >= 0 ? rawData.Substring(fileIndex + 2) : throw new Exception("Wrong server response");

            var list = rawData.Substring(0, fileIndex).Split(';');
            UInt64 fileSize = 0;
            DateTime modifyDate = DateTime.MinValue;
            EntryType type = EntryType.Unknown;

            foreach(var item in list)
            {
                var index = item.IndexOf('=');
                var factType = item.Substring(0, index);
                var value = item.Substring(index + 1);
                switch (factType.ToLower())
                {
                    case "size":
                        UInt64.TryParse(value, out fileSize);
                        break;
                    case "modify":
                        var dotIndex = value.IndexOf('.');
                        value = dotIndex >= 0 ? value.Substring(0, dotIndex) : value;
                        DateTime.TryParseExact(value, "yyyyMMddHHmmss", new DateTimeFormatInfo(), DateTimeStyles.None, out modifyDate); //"yyyyMMddHHmmss"
                        break;
                    case "type":
                        type = value == "file" ? EntryType.File : (value == "dir" ? EntryType.Directory : EntryType.Unknown);
                        break;
                }

            }

            
            
            return new FtpEntry(type, modifyDate, Path.Combine(parent, fileName), fileSize);
        }

    }
}
