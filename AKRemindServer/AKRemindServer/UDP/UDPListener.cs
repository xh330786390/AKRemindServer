using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace AKRemindClient.UDP
{
    public class AddMessageEventArgs : EventArgs
    {
        public string mess;//接收到的消息
    }

    /// <summary>
    /// UDP监听消息
    /// </summary>
    class UDPListener   
    {
        private Thread th;
        private EndPoint ep;
        private Socket sock;
        private string receiveData;
        public event EventHandler<AddMessageEventArgs> onAddMessage;

        public UDPListener() { }

        public void StartListen()
        {
            th = new Thread(new ThreadStart(Listen));
            th.Start();
        }

        public void Stop()
        {
            sock.Close();
            th.Abort();
        }

        private void Listen()
        {
            try
            {
                //定义socket对象
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9124);

                sock.Bind(iep);
                ep = (EndPoint)iep;
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    sock.ReceiveFrom(bytes, ref ep);
                    receiveData = System.Text.Encoding.Unicode.GetString(bytes);
                    receiveData = receiveData.TrimEnd('\u0000');
                    AddMessageEventArgs arg = new AddMessageEventArgs();
                    arg.mess = receiveData;
                    onAddMessage(this, arg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);  
            }
        }
    }
}
