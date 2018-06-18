using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public static class EnumerableExtension
    {
        public static string Join<T>(this IEnumerable<T> data, string joiner = null,string brace=null,Func<T,string> formater=null) {
            var sb = new StringBuilder();
            joiner = joiner ?? string.Empty;
            formater = formater ?? new Func<T, string>((item)=>item==null?string.Empty:item.ToString());
            foreach(var item in data) {
                if (sb.Length != 0) sb.Append(joiner);
                if (brace != null) sb.Append(brace);
                var valstr = formater(item);
                sb.Append(valstr == null?string.Empty: valstr);
                if (brace != null) sb.Append(brace);
            }
            return sb.ToString();
        }
    }
}
