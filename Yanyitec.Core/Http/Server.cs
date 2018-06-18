using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Yanyitec.Log;

namespace Yanyitec.Http
{
    public class Server
    {
        System.Net.Sockets.Socket _ListenSocket;

        /// <summary>
        /// 地址
        /// </summary>
        public IPAddress Address { get; private set; }
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; private set; }


        public ILogger Logger { get; private set; }

        public int Backlog { get; private set; }

        public System.Net.HttpListener _Listener;

        void Init() {
            
        }

        void InternalStart(IPAddress addr, int port) {
            var context = _Listener.GetContext();
            //context.Request.
            if (addr==null)
            {
                var hostname = Dns.GetHostName();
                var addrs = System.Net.Dns.GetHostAddresses(hostname);
                foreach (var ad in addrs) {
                    if (ad.GetAddressBytes().Length == 4) {
                        addr = ad;
                        break;
                    }
                }
            }
            if (addr == null) {
                throw new InvalidProgramException("Cannot bind ipaddress.");
            }
            this.Logger.Log( LogLevels.Information,"Address:" + addr.ToString());
            if (port == 0) {
                port = 8080;
            }
            this.Logger.Log(LogLevels.Information,"Port:" + port.ToString());
            var endpoint = new IPEndPoint(addr, port);
            this._ListenSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            this._ListenSocket.Bind(endpoint);
            this.Logger.Log(LogLevels.Information,"Bind:" + endpoint.ToString());
            this._ListenSocket.Listen(this.Backlog==0?5:this.Backlog);
            this.Logger.Log(LogLevels.Information,"Start Listen");

            while (true) {
                this._ListenSocket.AcceptAsync(this.AquireAcceptSocketAsyncEventArgs());
            }
            
        }

        protected internal AcceptSocketAsyncEventArgs AquireAcceptSocketAsyncEventArgs() {
            return new AcceptSocketAsyncEventArgs(this);
        }

        protected internal RecieveSocketAsyncEventArgs AquireRecieveSocketAsyncEventArgs()
        {
            return new RecieveSocketAsyncEventArgs(this);
        }

        protected internal Action<IRequest, IRespose> _HandleRequest {
            get;set;
        }

        public event Action<IRequest, IRespose> HandleRequest {
            add {
                this._HandleRequest += value;
            }
            remove {
                this._HandleRequest -= value;
            }
        }

        void Stop() {

        }

    }
}
