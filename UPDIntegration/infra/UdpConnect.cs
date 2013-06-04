using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPDIntegration
{
    public interface UdpConnect
    {
        int send(byte[] data, int length);
        byte[] receive();
        void disconnect();
        bool isConnect();
        void receiveAsynchronous(int milliseconds, Action<dynamic> method, Type methodType);
    }
}
