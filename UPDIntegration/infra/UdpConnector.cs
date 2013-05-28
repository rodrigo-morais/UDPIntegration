using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UPDIntegration
{
    public class UdpConnector : UdpConnect
    {
        private UdpClient server;
        private UdpClient client;
        private int port;

        public UdpConnector(string ipRemote, int portRemote, int port)
        {
            this.port = port;
            this.client = new UdpClient(ipRemote, portRemote);
            this.server = new UdpClient(port);
        }

        public int send(byte[] data, int length)
        {
            return this.client.Send(data, data.Length);
        }

        public byte[] receive()
        {
            try
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, this.port);
                return this.server.Receive(ref remoteEP);
            }
            catch
            {
                byte[] blank = new byte[0];
                return blank;
            }
        }

        public void disconnect()
        {
            this.client.Close();
            this.client = null;

            this.server.Close();
            this.server = null;
        }
    }
}
