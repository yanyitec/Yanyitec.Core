using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Meta
{
    public interface IProperty<T>:IProperty
        where T:class
    {
        new Func<T, bool, object> GetValue { get; }
        new Action<T,  object> SetValue { get; }

    }
}
