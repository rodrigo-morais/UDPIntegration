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
        public void testSendStringMessageCallReturnBytesLength()
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
        public void testSendShortMessageCallReturnBytesLength()
        {
            //Arrange
            short message = 5;
            var data = BitConverter.GetBytes(message);

            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.send(data, data.Length)).Returns(data.Length);

            var udp = new UdpIntegration(connector.Object);

            //Act
            int returnBytes = udp.send(message);

            //Assert
            Assert.AreEqual(data.Length, returnBytes);
        }

        [TestMethod]
        public void testSendMessageCallSendInConnector()
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

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void testSendMessageWithoutConnectionReturnException()
        {
            //Arrange
            String message = "Teste";
            var data = Encoding.ASCII.GetBytes(message);

            var connector = new UdpConnector("127.0.0.1", 9999, 9998);


            var udp = new UdpIntegration(connector);

            //Act and Assert
            udp.close();
            udp.send(message);
        }

        [TestMethod]
        public void testReceiveAsynWithMethodWithShortParameterCallOnceReceiveAsynchronousMethodInConnector()
        {
            //Arrange
            Action<short> method = (message) => { };

            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.receiveAsynchronous(1, It.IsAny<Action<dynamic>>(), typeof(Int16)));

            UdpIntegration integration = new UdpIntegration(connector.Object);
            

            //Act
            integration.receiveByTime(1, method);

            //Assert
            connector.Verify(conn => conn.receiveAsynchronous(1, It.IsAny<Action<dynamic>>(), typeof(Int16)), Times.Once());
        }

        [TestMethod]
        public void testReceiveAsynWithMethodWithStringParameterCallOnceReceiveAsynchronousMethodInConnector()
        {
            //Arrange
            Action<String> method = (message) => { };

            var connector = new Mock<UdpConnect>();
            connector.Setup(udpConnector => udpConnector.receiveAsynchronous(1, It.IsAny<Action<dynamic>>(), typeof(String)));

            UdpIntegration integration = new UdpIntegration(connector.Object);


            //Act
            integration.receiveByTime(1, method);

            //Assert
            connector.Verify(conn => conn.receiveAsynchronous(1, It.IsAny<Action<dynamic>>(), typeof(String)), Times.Once());
        }
    }
}
