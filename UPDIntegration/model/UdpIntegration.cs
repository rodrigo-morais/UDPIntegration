using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPDIntegration
{
    public class UdpIntegration
    {
        private UdpConnect connector;

        public UdpIntegration(UdpConnect connector)
        {
            this.connector = connector;
        }

        public int send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            int returnLength = connector.send(data, data.Length);
            return returnLength;
        }

        public byte[] receive()
        {
            var data = connector.receive();
            return data;
        }
    }
}
