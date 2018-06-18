using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Yanyitec
{
    public static class StringExtensions
    {
        static Regex CommentRegex = new Regex("\\s*//[^\\n]\\n",RegexOptions.Compiled);
        public static JToken ToJson(this string self) {
            var json = CommentRegex.Replace(self, string.Empty);
            return JToken.Parse(json);
        }

        public static T JDeserialize<T>(this string JSON) {
            var json = CommentRegex.Replace(JSON, string.Empty);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToEnum<T>(this string self) where T:struct {
            
            T result = default(T);
            Enum.TryParse<T>(self, out result);
            return result;
        }

        public static Nullable<T> ToNullableEnum<T>(this string self) where T : struct
        {

            T result = default(T);
            if(Enum.TryParse<T>(self, out result)) return result;
            return new Nullable<T>();
        }
    }
}
