using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Yanyitec.Http
{
    public class HttpRequest : IRequest
    {
        public HttpRequest(HttpListenerRequest internalRequest) {

            this.Arguments = new NameValueDictionary(internalRequest.QueryString);
            
            this.Cookies = new CookieDictionary(internalRequest.Cookies);
            this.Headers = new NameValueDictionary(internalRequest.Headers);
            if (internalRequest.ContentType == "application/json") {
                using (var textReader = new System.IO.StreamReader(internalRequest.InputStream)) {
                    using (var jsonReader = new Newtonsoft.Json.JsonTextReader(textReader))
                    {
                        var deserializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
                        this.Json= deserializer.Deserialize(jsonReader) as JToken;
                    }
                }
                
            }
            
        }

        public JToken Json { get; set; }

        public HttpListenerRequest internalRequest;
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HttpMethods Method { get; set; }

        public IStringDictionary Arguments { get; set; }

        public IStringDictionary Datas { get; set; }

        public IStringDictionary Cookies { get; set; }

        public IStringDictionary Headers { get; set; }
    }
}
