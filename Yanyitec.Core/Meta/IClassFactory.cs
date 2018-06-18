using System;

namespace Yanyitec.Meta
{
    public interface IClassFactory
    {
        IClass Aquire(Type type);
        IClass<T> Aquire<T>() where T : class;

        IClass Register(Type firstType,params Type[] types);

        IClass<VType> Register<KType, VType>() where VType:class;
    }
}