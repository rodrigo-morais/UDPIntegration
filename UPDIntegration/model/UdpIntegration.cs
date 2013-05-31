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
            try
            {
                var data = Encoding.UTF8.GetBytes(message);
                int returnLength = connector.send(data, data.Length);
                return returnLength;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int send(short message)
        {
            try
            {
                var data = BitConverter.GetBytes(message);
                int returnLength = connector.send(data, data.Length);
                return returnLength;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] receive()
        {
            try
            {
                var data = connector.receive();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void close()
        {
            this.connector.disconnect();
        }
    }
}
