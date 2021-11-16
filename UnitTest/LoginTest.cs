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
            var client = new Client(mockConnection.Object);

            client.Login("michaldworczyk", "12345");
            
        }

        [TestMethod]
        public void WrongUserTest()
        {
            var mockConnection = new Mock<IFtpConnection>();
            mockConnection.Setup((x) => x.SendRequest(It.IsAny<string>())).Returns(new FtpResponse(500, "NOT OK"));
            var client = new Client(mockConnection.Object);

            var ex = Assert.ThrowsException<AuthenticationException>(() => client.Login("michaldworczyk", "12345"));
        }
    }
}
