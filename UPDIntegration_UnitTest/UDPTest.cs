using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UPDIntegration;
using Moq;
using System.Net;

namespace UPDIntegration_UnitTest
{
    [TestClass]
    public class UdpTest
    {
        [TestMethod]
        public void testSendMessageCallReturnBytesLength()
        {
            //Arrange
            String message = "Teste";
            var data = Encoding.ASCII.GetBytes(message);
            
            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.send(data, data.Length)).Returns(data.Length);
            
            var udp = new UdpIntegration(connector.Object);

            //Act
            int returnBytes = udp.send(message);

            //Assert
            Assert.AreEqual(data.Length, returnBytes);
        }

        [TestMethod]
        public void testSendMessageCallSend()
        {
            //Arrange
            String message = "Teste";
            var data = Encoding.ASCII.GetBytes(message);

            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.send(data, data.Length)).Returns(data.Length);


            var udp = new UdpIntegration(connector.Object);

            //Act
            int returnBytes = udp.send(message);

            //Assert
            connector.Verify(conn => conn.send(data, data.Length), Times.Once());
        }

        [TestMethod]
        public void testReceiveMessageCallReturnBytes()
        {
            //Arrange
            String message = "Teste";
            var data = Encoding.ASCII.GetBytes(message);

            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.receive()).Returns(data);

            var udp = new UdpIntegration(connector.Object);

            //Act
            var returnBytes = udp.receive();

            //Assert
            Assert.AreEqual(data, returnBytes);
        }
    }
}
