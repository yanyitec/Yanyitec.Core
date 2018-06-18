using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Meta
{
    public interface IProperty
    {
        IClass Class { get; }

        bool IsNullable { get;  }
        bool IsEnumerable { get;}

        Type EntitativeType { get;  }

        Type KeyType { get; }

        string Name { get; }

        Func<object,bool,object> GetValue { get; }
        Action<object, object> SetValue { get; }

        IClass SubsidaryClass { get; }



    }
}
