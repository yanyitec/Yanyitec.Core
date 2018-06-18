using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Yanyitec.Meta
{
    public class ClassFactory : IClassFactory
    {
        ConcurrentDictionary<Guid, IClass> _Classes = new ConcurrentDictionary<Guid, IClass>();

        public ClassFactory(Func<Type, IClass> ClassFactory = null) {
            
        }
        

        public IClass<T> Aquire<T>()where T:class {
            return _Classes.GetOrAdd(typeof(T).GUID, (id) => new Class<T>(this) ) as IClass<T>;
        }
        static Type ClassType = typeof(Class<>);
        static object[] EmptyObjectArray = new object[0];
        static MethodInfo CreateTypedClassMethodInfo = typeof(ClassFactory).GetMethod("CreateTypedClass");
        protected IClass CreateClass(Type type) {
            var method = CreateTypedClassMethodInfo.MakeGenericMethod(type);
            return method.Invoke(this, EmptyObjectArray) as IClass;
        }

        protected virtual IClass<T> CreateTypedClass<T>() where T:class{
            return new Class<T>(this);
        }
        public IClass Aquire(Type type) {
            return _Classes.GetOrAdd(type.GUID,(id)=> CreateClass(type));
        }

        public IClass Register(Type firstType, params Type[] types) {
            var vType = types.Length == 0 ? firstType : types[types.Length-1];
            
            if (types.Length == 0) {
                return this._Classes.AddOrUpdate(firstType.GUID, (tid)=>CreateClass(vType), (tid, oldt) => oldt.ObjectType.GUID==tid?oldt:CreateClass(vType));
            }
            var vClass = this._Classes.AddOrUpdate(firstType.GUID, (tid) => CreateClass(vType), (tid, oldt) => oldt.ObjectType.GUID == tid ? oldt : CreateClass(vType));
            this._Classes.AddOrUpdate(firstType.GUID,vClass,(tid,old)=>vClass);
            foreach (var type in types) {
                this._Classes.AddOrUpdate(type.GUID, vClass, (tid, old) => vClass);
            }
            return vClass;
        }
        public IClass<VType> Register<KType, VType>() where VType:class {
            return (IClass<VType>)this._Classes.AddOrUpdate(typeof(KType).GUID, (tid) => CreateTypedClass<VType>(), (tid, oldt) => oldt.ObjectType.GUID == tid ? (IClass<VType>)oldt : CreateTypedClass<VType>());
        }

        public static IClassFactory Default = new ClassFactory();
    }
}
