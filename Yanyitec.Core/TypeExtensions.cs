using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq.Expressions;

namespace Yanyitec
{
    public static class TypeExtensions
    {
        public static Type GetSubstantiveType(this Type self) {
            if (self.FullName.StartsWith("System.Nullable`1")) {
                return self.GetGenericArguments()[0];
            }
            return self;
        }

        public static object GetValue(this Type self, string text) {
            if (self == typeof(string)) return text;
            Func<string, object> parser = null;
            if (self.IsClass)
            {
                parser = ClassParsers.GetOrAdd(self.GUID, (tid) =>
                {
                    //var method = DeserializeObjectMethodInfo.MakeGenericMethod(self);
                    var method = typeof(StringExtensions).GetMethod("JDeserialize");
                    method = method.MakeGenericMethod(self);
                    var txtExpr = Expression.Parameter(typeof(string));
                    
                    var expr = Expression.Lambda<Func<string, object>>(
                        Expression.Convert(Expression.Call(null, method, txtExpr), typeof(object)),
                        txtExpr
                        );
                    return expr.Compile();
                });
                return parser(text);
            }
            else if (self.IsEnum) {
                parser = ClassParsers.GetOrAdd(self.GUID, (tid) =>
                {
                    //var method = DeserializeObjectMethodInfo.MakeGenericMethod(self);
                    var method = typeof(StringExtensions).GetMethod("ToEnum");
                    method = method.MakeGenericMethod(self);
                    var txtExpr = Expression.Parameter(typeof(string));

                    var expr = Expression.Lambda<Func<string, object>>(
                        Expression.Convert(Expression.Call(null, method, txtExpr), typeof(object)),
                        txtExpr
                        );
                    return expr.Compile();
                });
                return parser(text);
            }
            if (self.FullName.StartsWith("System.Nullable`1")) {
                var actualType = self.GetGenericArguments()[0];
                if (actualType.IsEnum)
                {
                    parser = ClassParsers.GetOrAdd(self.GUID, (tid) =>
                    {
                        //var method = DeserializeObjectMethodInfo.MakeGenericMethod(self);
                        var method = typeof(StringExtensions).GetMethod("ToNullableEnum");
                        method = method.MakeGenericMethod(self);
                        var txtExpr = Expression.Parameter(typeof(string));

                        var expr = Expression.Lambda<Func<string, object>>(
                            Expression.Convert(Expression.Call(null, method, txtExpr), typeof(object)),
                            txtExpr
                            );
                        return expr.Compile();
                    });
                    return parser(text);
                }
                else {
                    NullableValueParsers.TryGetValue(self.GUID,out parser);
                    return parser(text);
                }
            }
            NonullableValueParsers.TryGetValue(self.GUID, out parser);
            return parser(text);
            
        }
        public static object GetDefaultValue(this Type self) {
            
            if (self.IsClass) return null;
            if (self.IsEnum) return Enum.GetValues(self).GetValue(0);
            object dftValue = null;
            if (self.FullName.StartsWith("System.Nullable`1"))
            {
                var actualType = self.GetGenericArguments()[0];
                if (!DefaultNonullableValues.TryGetValue(actualType.GUID, out dftValue)) {
                    if (actualType.IsEnum) {
                        return NullableEnumValues.GetOrAdd(actualType.GUID,(tid)=> {
                            return Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(self));
                        });
                    }
                }
            }
            else {
                DefaultNullableValues.TryGetValue(self.GUID, out dftValue);
            }
            return dftValue;
        }
        static MethodInfo GetDeserializeMethod() {
            //Newtonsoft.Json.JsonConvert.DeserializeObject
            var methods = typeof(Newtonsoft.Json.JsonConvert).GetMethods(BindingFlags.Static);
            foreach (var method in methods) {
                if (method.Name == "DeserializeObject" && method.GetGenericArguments().Length == 1) return method;
            }
            return null;
        }
        static MethodInfo _DeserializeMethod;
        static MethodInfo DeserializeObjectMethodInfo {
            get {
                if (_DeserializeMethod == null) {
                    lock (ClassParsers) {
                        if (_DeserializeMethod == null) _DeserializeMethod = GetDeserializeMethod();
                    }
                }
                return _DeserializeMethod;
            }
        }

        static ConcurrentDictionary<Guid, object> NullableEnumValues = new ConcurrentDictionary<Guid, object>();

        static ConcurrentDictionary<Guid, Func<string,object>> ClassParsers = new ConcurrentDictionary<Guid, Func<string,object>>();
        //static ConcurrentDictionary<Guid, Func<string, object>> ClassParsers = new ConcurrentDictionary<Guid, Func<string, object>>();


        public static IReadOnlyDictionary<Guid, object> DefaultNullableValues = new SortedDictionary<Guid, object>()
        {
            { typeof(bool).GUID,new Nullable<bool>()},
            { typeof(char).GUID,new Nullable<char>()},
            { typeof(byte).GUID,new Nullable<byte>()},
            { typeof(short).GUID,new Nullable<short>()},
            { typeof(ushort).GUID,new Nullable<ushort>()},
            { typeof(int).GUID,new Nullable<int>()},
            { typeof(uint).GUID,new Nullable<uint>()},
            { typeof(long).GUID,new Nullable<long>()},
            { typeof(ulong).GUID,new Nullable<ulong>()},
            { typeof(float).GUID,new Nullable<float>()},
            { typeof(double).GUID,new Nullable<double>()},
            { typeof(decimal).GUID,new Nullable<decimal>()},
            { typeof(DateTime).GUID,new Nullable<DateTime>(DateTime.MinValue)}
        };

        static IReadOnlyDictionary<Guid, object> DefaultNonullableValues = new SortedDictionary<Guid, object>() {
            { typeof(bool).GUID,false},
            { typeof(char).GUID,'\0'},
            { typeof(byte).GUID,(byte)0},
            { typeof(short).GUID,(short)0},
            { typeof(ushort).GUID,(ushort)0},
            { typeof(int).GUID,0},
            { typeof(uint).GUID,(uint)0},
            { typeof(long).GUID,(long)0},
            { typeof(ulong).GUID,(ulong)0},
            { typeof(float).GUID,(float)0},
            { typeof(double).GUID,(double)0},
            { typeof(decimal).GUID,(decimal)0},
            { typeof(DateTime).GUID,DateTime.MinValue}
        };

        public static IReadOnlyDictionary<Guid, Func<string, object>> NonullableValueParsers = new Dictionary<Guid, Func<string, object>>() {
             { typeof(bool).GUID,(txt)=>{
                    if (string.IsNullOrWhiteSpace(txt)) return false;
                    txt = txt.Trim();
                    if(txt=="0" || txt=="false"||txt=="FALSE" || txt=="off" || txt=="off") return false;
                    
                    return true;
                 }
             },
            { typeof(char).GUID,(txt)=>string.IsNullOrEmpty(txt)?'\0':txt[0] },
            { typeof(byte).GUID,(txt)=>{ byte rs = 0; byte.TryParse(txt,out rs);return rs; } },
            { typeof(short).GUID,(txt)=>{ short rs = 0; short.TryParse(txt,out rs);return rs; }},
            { typeof(ushort).GUID,(txt)=>{ ushort rs = 0; ushort.TryParse(txt,out rs);return rs; }},
            { typeof(int).GUID,(txt)=>{ int rs = 0; int.TryParse(txt,out rs);return rs; }},
            { typeof(uint).GUID,(txt)=>{ uint rs = 0; uint.TryParse(txt,out rs);return rs; }},
            { typeof(long).GUID,(txt)=>{ long rs = 0; long.TryParse(txt,out rs);return rs; }},
            { typeof(ulong).GUID,(txt)=>{ ulong rs = 0; ulong.TryParse(txt,out rs);return rs; }},
            { typeof(float).GUID,(txt)=>{ float rs = 0; float.TryParse(txt,out rs);return rs; }},
            { typeof(double).GUID,(txt)=>{ double rs = 0; double.TryParse(txt,out rs);return rs; }},
            { typeof(decimal).GUID,(txt)=>{ decimal rs = 0; decimal.TryParse(txt,out rs);return rs; }},
            { typeof(DateTime).GUID,(txt)=>{ DateTime rs = DateTime.MinValue; DateTime.TryParse(txt,out rs);return rs; }}
        };

        public static IReadOnlyDictionary<Guid, Func<string, object>> NullableValueParsers = new Dictionary<Guid, Func<string, object>>() {
             { typeof(bool).GUID,(txt)=>{
                    if (string.IsNullOrWhiteSpace(txt)) return new Nullable<bool>();
                    txt = txt.Trim();
                    if(txt=="0" || txt=="false"||txt=="FALSE" || txt=="off" || txt=="off") return new Nullable<bool>(false);
                    return new Nullable<bool>(true);
                 }
             },
            { typeof(char).GUID,(txt)=>string.IsNullOrEmpty(txt)?new Nullable<char>(): new Nullable<char>(txt[0]) },
            { typeof(byte).GUID,(txt)=>{ byte rs = 0; if(byte.TryParse(txt,out rs)) return new Nullable<byte>(rs);return new Nullable<byte>(); } },
            { typeof(short).GUID,(txt)=>{ short rs = 0; if(short.TryParse(txt,out rs)) return new Nullable<short>(rs);return new Nullable<short>(); }},
            { typeof(ushort).GUID,(txt)=>{ ushort rs = 0; if(ushort.TryParse(txt,out rs)) return new Nullable<ushort>(rs);return new Nullable<ushort>(); }},
            { typeof(int).GUID,(txt)=>{ int rs = 0; if(int.TryParse(txt,out rs)) return new Nullable<int>(rs);return new Nullable<int>(); }},
            { typeof(uint).GUID,(txt)=>{ uint rs = 0; if(uint.TryParse(txt,out rs)) return new Nullable<uint>(rs);return new Nullable<uint>(); }},
            { typeof(long).GUID,(txt)=>{ long rs = 0; if(long.TryParse(txt,out rs)) return new Nullable<long>(rs);return new Nullable<long>(); }},
            { typeof(ulong).GUID,(txt)=>{ ulong rs = 0; if(ulong.TryParse(txt,out rs)) return new Nullable<ulong>(rs);return new Nullable<ulong>(); }},
            { typeof(float).GUID,(txt)=>{ float rs = 0; if(float.TryParse(txt,out rs)) return new Nullable<float>(rs);return new Nullable<float>(); }},
            { typeof(double).GUID,(txt)=>{ double rs = 0; if(double.TryParse(txt,out rs)) return new Nullable<double>(rs);return new Nullable<double>(); }},
            { typeof(decimal).GUID,(txt)=>{ decimal rs = 0; if(decimal.TryParse(txt,out rs)) return new Nullable<decimal>(rs);return new Nullable<decimal>(); }},
            { typeof(DateTime).GUID,(txt)=>{ DateTime rs = DateTime.MinValue; if(DateTime.TryParse(txt,out rs)) return new Nullable<DateTime>(rs);return new Nullable<DateTime>(); }}
        };
    }
}
