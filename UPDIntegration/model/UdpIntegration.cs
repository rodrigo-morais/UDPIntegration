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
        
        public int send(dynamic message)
        {
            try
            {
                Type type = message.GetType();
                dynamic data;

                if (type == typeof(String))
                {
                    data = Encoding.UTF8.GetBytes(message);
                }
                else
                {
                    data = BitConverter.GetBytes(message);
                }
                
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

        public void receiveByTime<T>(int milliseconds, Action<T> method)
        {
            try
            {
                Action<dynamic> methodAction = (n) => method(n);
                Type methodType = method.GetType().GetGenericArguments()[0];

                connector.receiveAsynchronous(milliseconds, methodAction, methodType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void close()
        {
            try
            {
                this.connector.disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
