using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public static class JObjectExtension
    {

        public static JToken MergeObject(this JObject self, JObject other)
        {
            if (other == null || other.Type == JTokenType.Undefined || other.Type == JTokenType.Null) return self == null ? new JObject() : self.DeepClone();
            if (self == null || self.Type == JTokenType.Undefined || self.Type == JTokenType.Null) self = new JObject();
            var dest = new JObject();
            foreach (var pair in other)
            {
                MergeObject(dest, self, other, pair.Key);
            }
            return dest;
        }
        static JToken MergeObject(JObject dest, JObject self, JObject other, string fieldname)
        {
            JToken selfValue = null;
            self.TryGetValue(fieldname, out selfValue);
            JToken otherValue = null;
            other.TryGetValue(fieldname, out otherValue);

            if (selfValue == null || selfValue.Type == JTokenType.Undefined || selfValue.Type == JTokenType.Null)
            {
                return dest[fieldname] = otherValue;
            }
            if (selfValue.Type == JTokenType.Array) {
                return dest[fieldname] = selfValue.DeepClone();
            }

            if (otherValue == null || otherValue.Type == JTokenType.Undefined )
            {
                return dest[fieldname];
            }

            if (selfValue.Type == JTokenType.Object && otherValue.Type == JTokenType.Object) {
                return dest[fieldname] = (selfValue as JObject).MergeObject(otherValue as JObject);
            }

            return dest[fieldname] = otherValue.DeepClone();
        }

        public static JObject ConvertTo(this JObject data, JObject schema) {
            var result = new JObject();
            foreach (var pair in schema) {
                ConvertTo(result,data,pair.Value,pair.Key,0);
            }
            return result;
        }

        static void ConvertTo(JToken dest, JObject data, JToken importValue, string importKey,int importArrIndex) {
            if (importValue.Type == JTokenType.String)
            {
                var importString = importValue.ToString();
                if (importString.Length > 3 && importString[0] == '$' && importString[1] == '{' && importString[importString.Length - 1] == '}')
                {
                    var expr = importString.Substring(2, importString.Length - 3);
                    importValue = data.SelectToken(expr);
                }


            }
            else if (importValue.Type == JTokenType.Object)
            {
                var val = new JObject();
                var schema = importValue as JObject;
                foreach (var pair in schema)
                {
                    ConvertTo(val, data, pair.Value, pair.Key, 0);
                }
                importValue = val;
            }
            else if (importValue.Type == JTokenType.Array)
            {
                var val = new JArray();
                var schema = importValue as JArray;
                for (int i = 0, j = schema.Count; i < j; i++)
                {
                    ConvertTo(val, data, schema[i], null, i);
                }
                importValue = val;
            }
            else if (importValue == null) {
                importValue = JValue.CreateUndefined();
            } else {
                importValue = importValue.DeepClone();
            }

            if (importKey != null)
            {
                (dest as JObject).Add(importKey, importValue);
            }
            else
            {
                (dest as JArray).Add(importValue);
            }

        }
    }
}
 