using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public class StringDictionary : Dictionary<string, string>, IStringDictionary
    {
        public new string this[string key] {
            get {
                string value = null;
                base.TryGetValue(key,out value);
                return value;
            }
            set {
                if (base.ContainsKey(key))
                {
                    base[key] = value;
                }
                else {
                    base.Add(key,value);
                }
            }
        }
    }
}
