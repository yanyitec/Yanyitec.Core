using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Yanyitec.Meta
{
    public class Class<T> : Class, IClass<T>
        where T : class
    {

        public Class(IClassFactory factory)
            : base(typeof(T),factory)
        {
            

        }
        protected override IProperty CreateProperty(MemberInfo memberInfo)
        {
            return new Property<T>(memberInfo,this);
        }



        public new IProperty<T> this[string memberName] {
            get
            {

                IProperty member = base[memberName];
                return member as IProperty<T>;
            }
        }

        IEnumerator<IProperty<T>> IEnumerable<IProperty<T>>.GetEnumerator()
        {
            return new Enumerator(base.GetEnumerator());
        }

        public class Enumerator : IEnumerator<IProperty<T>>
        {
            IEnumerator<IProperty> Internal;
            public Enumerator(IEnumerator<IProperty> internalen) {
                this.Internal = internalen;
            }
            public IProperty<T> Current => this.Internal.Current as IProperty<T>;

            object IEnumerator.Current => this.Internal.Current;

            public void Dispose()
            {
                this.Internal.Dispose();
            }

            public bool MoveNext()
            {
                return this.Internal.MoveNext();
            }

            public void Reset()
            {
                this.Internal.Reset();
            }
        }


    
        
    }
}
