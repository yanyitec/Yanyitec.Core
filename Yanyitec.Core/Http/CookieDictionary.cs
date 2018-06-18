using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Yanyitec.Http
{
    public class CookieDictionary :IStringDictionary
    {
        public CookieDictionary(CookieCollection raw) {
            this.Raw = raw;
        }

        public string this[string key] {
            get {
                var cookie = this.Raw[key];
                return cookie == null ? null:cookie.Value;
            }
            set {
                this.Raw.Add(new Cookie(key,value));
            }
        }

        public CookieCollection Raw { get; private set; }
    }
}
