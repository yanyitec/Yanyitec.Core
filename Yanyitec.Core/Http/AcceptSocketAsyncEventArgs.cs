using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Yanyitec.Http
{
    public class AcceptSocketAsyncEventArgs : SocketAsyncEventArgs
    {
        public AcceptSocketAsyncEventArgs(Server server) {
            this.Server = server;
        }
        public Server Server { get; private set; }
        protected override void OnCompleted(SocketAsyncEventArgs e)
        {
            e.AcceptSocket.ReceiveAsync(this.Server.AquireRecieveSocketAsyncEventArgs());
        }
    }
}
