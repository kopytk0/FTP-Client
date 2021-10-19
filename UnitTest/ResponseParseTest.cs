using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FTP;

namespace UnitTest
{
    [TestClass]
    public class ResponseParseTest
    {
        [TestMethod]
        public void SuccessCodeParseTest()
        {
            var response = FTPResponse.Parse("123 lol 321");
            Assert.AreEqual(123, response.Code);
            Assert.AreEqual("lol 321", response.Message);
            Assert.IsTrue(response.IsSuccess());
        }

        [TestMethod]

        public void ErrorCodeParseTest()
        {
            var response = FTPResponse.Parse("500 server exploded");
            Assert.AreEqual(500, response.Code);
            Assert.AreEqual("server exploded", response.Message);
            Assert.IsFalse(response.IsSuccess());
        }

        [TestMethod]

        public void InvalidDataParseTest()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => FTPResponse.Parse("69"));
            Assert.IsNotNull(ex.Message);
        }
    }
}
