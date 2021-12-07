using System;
using FTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class FtpEntryParseTest
    {
        [TestMethod]
        public void SuccessParseTest()
        {
            var ftpEntry = FtpEntry.Parse("type=file;modify=20150803062903.123;size=1410887680; filename.avi",
                "rodzic");
            Assert.AreEqual("filename.avi", ftpEntry.Name);
            Assert.AreEqual(new DateTime(2015, 08, 03, 6, 29, 3), ftpEntry.ModifyDate);
            Assert.AreEqual((ulong)1410887680, ftpEntry.FileSize);
            Assert.AreEqual(@"rodzic\filename.avi", ftpEntry.FullPath);
            Assert.AreEqual(FtpEntry.EntryType.File, ftpEntry.Type);
        }
    }
}