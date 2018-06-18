using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Yanyitec.Http
{
    public class NameValueDictionary : IStringDictionary
    {
        public NameValueDictionary(NameValueCollection collection) {
            this.Raw = collection;
        }
        public NameValueCollection Raw { get; private set; }
        public string this[string key] {
            get {
                return this.Raw[key];
            }
            set {
                this.Raw[key] = value;
            }
        }
    }
}
