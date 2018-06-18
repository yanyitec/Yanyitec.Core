using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Yanyitec.Meta
{
    public interface IClass<T>:IClass,IEnumerable<IProperty<T>>
        where T:class 
    {

        

        
        new IProperty<T> this[string memberName] { get; }
    }
}
