using System;
using System.Globalization;
using System.IO;

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

        internal FtpEntry(EntryType type, DateTime creationDate, string fullPath, ulong fileSize)
        {
            Type = type;
            ModifyDate = creationDate;
            FullPath = fullPath;
            FileSize = fileSize;
            var indyk = FullPath.LastIndexOf('\\');
            Name = indyk < 0 ? FullPath : FullPath.Substring(indyk + 1);
        }

        public EntryType Type { get; }
        public DateTime ModifyDate { get; }
        public string Name { get; }
        public string FullPath { get; }
        public ulong FileSize { get; }

        internal static FtpEntry Parse(string rawData, string parent = "")
        {
            var fileIndex = rawData.IndexOf("; ");
            var fileName = fileIndex >= 0
                ? rawData.Substring(fileIndex + 2)
                : throw new Exception("Wrong server response");

            var list = rawData.Substring(0, fileIndex).Split(';');
            ulong fileSize = 0;
            var modifyDate = DateTime.MinValue;
            var type = EntryType.Unknown;

            foreach (var item in list)
            {
                var index = item.IndexOf('=');
                var factType = item.Substring(0, index);
                var value = item.Substring(index + 1);
                switch (factType.ToLower())
                {
                    case "size":
                        ulong.TryParse(value, out fileSize);
                        break;
                    case "modify":
                        var dotIndex = value.IndexOf('.');
                        value = dotIndex >= 0 ? value.Substring(0, dotIndex) : value;
                        modifyDate = ParseMlsdDate(value);
                        break;
                    case "type":
                        type = value == "file" ? EntryType.File :
                            value == "dir" ? EntryType.Directory : EntryType.Unknown;
                        break;
                }
            }

            return new FtpEntry(type, modifyDate, Path.Combine(parent, fileName), fileSize);
        }

        private static DateTime ParseMlsdDate(string value)
        {
            DateTime modifyDate;
            DateTime.TryParseExact(value, "yyyyMMddHHmmss", new DateTimeFormatInfo(), DateTimeStyles.None,
                out modifyDate); //"yyyyMMddHHmmss"
            return modifyDate;
        }
    }
}