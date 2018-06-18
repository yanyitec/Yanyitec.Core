using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Yanyitec.Meta
{
    public class Class : IClass
    {
        public Class(Type objectType, IClassFactory factory) {
            this.ObjectType = ObjectType;
            this.ClassFactory = factory;
        }

        public IClassFactory ClassFactory { get;private set; }

        protected virtual IProperty CreateProperty(MemberInfo memberInfo) {
            return new Property(memberInfo,this);
        }
        public Type ObjectType { get; private set; }

        protected Dictionary<string, IProperty> _Props;

        public IProperty this[string memberName] {
            get
            {
                if (_Props == null) {
                    lock (this) {
                        if (this._Props == null) {
                            InitMembers();
                        }
                    }
                }
                IProperty member = null;
                this._Props.TryGetValue(memberName,out member);
                return member;
            }
        }

        public IEnumerable<string> PropertyNames {
            get {
                if (this._Props == null)
                {
                    lock (this)
                    {
                        if (this._Props == null)
                        {
                            InitMembers();
                        }
                    }
                }
                return _Props.Keys;
            }
        }

        protected virtual void InitMembers() {
            var members = this.ObjectType.GetMembers();
            var result = new Dictionary<string, IProperty>();
            foreach(MemberInfo member in members) {
                if (member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)
                {
                    var prop = this.CreateProperty(member);
                    if(prop!=null)result.Add(member.Name, prop);
                }
            }
            this._Props = result;
        }

        //ConcurrentDictionary<string, Visitor> _Visitors;
        //public Visitor AquireVisitor(string expr) {
        //    if (this._Visitors == null) {
        //        lock (this) {
        //            if (this._Visitors == null) {
        //                this._Visitors = new ConcurrentDictionary<string, Visitor>();
        //            }
        //        }
        //    }
        //    return this._Visitors.GetOrAdd(expr,(e)=> Visitor.Parse(expr,this));
        //}

        public IEnumerator<IProperty> GetEnumerator()
        {
            return this._Props.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._Props.Values.GetEnumerator();
        }

        
    }
}
