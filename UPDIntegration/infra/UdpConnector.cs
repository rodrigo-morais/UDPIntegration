using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;

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
        private bool connected;

        private IAsyncResult result;
        private IPEndPoint remoteEP;
        private Timer waitReceiveConnect;
        private Action<dynamic> method;
        private Type methodType;

        public UdpConnector(string ipRemote, int portRemote, int port)
        {
            this.ipRemote = ipRemote;
            this.portRemote = portRemote;
            this.port = port;
            this.client = new UdpClient(ipRemote, portRemote);
            this.server = new UdpClient(port);
            this.localIP = this.localIPAddress();
            this.connected = true;

            remoteEP = new IPEndPoint(IPAddress.Any, this.portRemote);
        }

        public int send(byte[] data, int length)
        {
            if (this.connected)
            {
                return this.client.Send(data, data.Length);
            }
            else
            {
                throw new Exception("UDP disconected;");
            }
        }

        public byte[] receive()
        {
            try
            {
                if (this.connected)
                {
                    return this.server.Receive(ref remoteEP);
                }
                else
                {
                    throw new Exception("UDP disconected;");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void disconnect()
        {
            try
            {
                this.client.Close();
                this.client = null;

                this.server.Close();
                this.server = null;

                this.connected = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public bool isConnect()
        {
            return connected;
        }

        public void receiveAsynchronous(int milliseconds, Action<dynamic> method, Type methodType)
        {
            try
            {
                if (result == null)
                {

                    this.method = method;
                    this.methodType = methodType;

                    result = this.server.BeginReceive(new AsyncCallback(asyncMethod), remoteEP); result = this.server.BeginReceive(delegate { asyncMethod(null); }, remoteEP);

                    waitReceiveConnect = new Timer();

                    waitReceiveConnect.Interval = milliseconds;
                    waitReceiveConnect.Enabled = true;
                    waitReceiveConnect.Elapsed += new ElapsedEventHandler(endReceive);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void endReceive(object source, ElapsedEventArgs e)
        {
            try
            {
                if (result != null)
                {
                    if (waitReceiveConnect != null)
                    {
                        waitReceiveConnect.Enabled = false;
                        waitReceiveConnect.Close();
                        waitReceiveConnect = null;
                    }


                    if (this.server != null)
                    {
                        if (this.server.Client.Connected)
                        {
                            this.server.EndReceive(result, ref remoteEP);
                            this.result = null;
                        }
                        else
                        {
                            this.result = null;
                        }
                    }
                    else
                    {
                        this.result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.result = null;
                throw ex;
            }
        }

        private void asyncMethod(IAsyncResult result)
        {
            try
            {
                if (result != null)
                {
                    if (waitReceiveConnect != null)
                    {
                        waitReceiveConnect.Enabled = false;
                        waitReceiveConnect.Close();
                        waitReceiveConnect = null;
                    }

                    if (this.server != null)
                    {
                        byte[] data = this.server.EndReceive(result, ref remoteEP);
                        dynamic message;

                        if (this.methodType == typeof(String))
                        {
                            message = Encoding.ASCII.GetString(data, 0, data.Length);
                        }
                        else
                        {
                            message = BitConverter.ToInt16(data, 0);
                        }

                        this.result = null;



                        this.method(message);
                    }
                    else
                    {
                        this.result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.result = null;
            }
        }
    }
}
