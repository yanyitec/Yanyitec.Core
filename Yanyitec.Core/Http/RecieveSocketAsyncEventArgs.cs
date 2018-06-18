using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Yanyitec.Http
{
    public class RecieveSocketAsyncEventArgs : SocketAsyncEventArgs
    {
        public RecieveSocketAsyncEventArgs(Server server) {
            this.Server = server;
           // this.Request = new HttpRequest();
        }
        public Server Server { get; private set; }

        public HttpRequest Request { get; set; }

        

        



        protected override void OnCompleted(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0) {
                this.Server._HandleRequest(this.Request,null);
                return;
            }
            
        }

        #region header parser
        bool HasCR { get; set; }
        bool HasLF { get; set; }

        List<ArraySegment<byte>> _Caches;

        int _CacheByteCount;

        void ParseHeader(byte[] buffer, int size) {
            for (var i = 0; i < size; i++)
            {
                byte b = buffer[i];
                if (b == '\r')
                {
                    if (this.HasCR)
                    {
                        throw new ArgumentException("Already have a \r.");
                    }
                    this.HasCR = true;
                    continue;
                }
                if (b == '\n')
                {
                    if (!this.HasCR)
                    {
                        throw new ArgumentException("Expect \r.");
                    }

                }

            }
        }

        void ParseHeaderLine(byte[] buffer, int startAt,int endAt) {
            string line = null;
            if (this._Caches.Count > 0)
            {
                line = GetString(buffer, endAt);
                
            }
            else {
                line = ASCIIEncoding.ASCII.GetString(buffer,startAt,endAt);
            }

            if (this.Request.Method == HttpMethods.UNKNOWN) {
                ParseHeaderCommandLine(line);
            }
        }

        void ParseHeaderCommandLine(string cmdLine) {
            var vs = cmdLine.Split(' ');
            var methodName = vs[0].ToUpper();
            var url = vs[1];

        }

        string GetString(byte[] buffer, int endAt) {
            var strBuffer = new byte[this._CacheByteCount + endAt];
            var index = 0;
            foreach (var seg in this._Caches) {
                seg.CopyTo(strBuffer, index);
                index += seg.Count;
            }
            Array.Copy(buffer, strBuffer, endAt);
            this._Caches.Clear();
            this._CacheByteCount = 0;

            return ASCIIEncoding.ASCII.GetString(strBuffer);
        }


        #endregion
    }
}
