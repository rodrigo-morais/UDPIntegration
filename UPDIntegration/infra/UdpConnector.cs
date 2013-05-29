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
        private string ipRemote;
        private int portRemote;
        private string localIP;

        public UdpConnector(string ipRemote, int portRemote, int port)
        {
            this.ipRemote = ipRemote;
            this.portRemote = portRemote;
            this.port = port;
            this.client = new UdpClient(ipRemote, portRemote);
            this.server = new UdpClient(port);
            this.localIP = this.localIPAddress();
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

        private string localIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    break;
                }
                else localIP = null;
            }
            return localIP;
        }

        public string getLocalIP()
        {
            return this.localIP;
        }

        public int getPort()
        {
            return this.port;
        }

        public string getIPRemote()
        {
            return this.ipRemote;
        }

        public int getPortRemote()
        {
            return this.portRemote;
        }
    }
}
