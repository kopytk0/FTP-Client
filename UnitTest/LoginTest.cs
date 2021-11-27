using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using FTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void SuccessLoginTest()
        {
            var mockConnection = new Mock<IFtpConnection>();
            mockConnection.Setup((x) => x.SendRequest(It.Is<string>(y => y.StartsWith("user", StringComparison.OrdinalIgnoreCase)))).Returns(new FtpResponse(200, "OKOK"));
            mockConnection.Setup(x => x.SendRequest(It.Is<string>(y => y.StartsWith("pass", StringComparison.OrdinalIgnoreCase)))).Returns(new FtpResponse(200, "OK"));
            var client = new Client(mockConnection.Object);

            Assert.IsTrue(client.Login("michaldworczyk", "12345").IsSuccess());
            
        }

        [TestMethod]
        public void WrongUserTest()
        {
            var mockConnection = new Mock<IFtpConnection>();
            mockConnection.Setup((x) => x.SendRequest(It.IsAny<string>())).Returns(new FtpResponse(500, "NOT OK"));
            var client = new Client(mockConnection.Object);

            var ex = Assert.ThrowsException<FtpException>(() => client.Login("michaldworczyk", "12345"));
        }
    }
}
