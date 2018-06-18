using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.DI
{
    public class DependentItem
    {
        Dictionary<string, DependentItem> _NamedItems;
        Dictionary<Guid, DependentItem> _TypedItems;
        DependentItem _ContainerItem;

        public string NominalName { get; set; }
        public Guid NominalTypeId { get; set; }
        public Type SubstantiveType { get; set; }

        Func<object> _CreateInstance;



        public DependenceLifecycles Lifecycle { get; set; }

        public DependentItem(DependentItem container,string name,object constantValue,Type valueType=null) {
            this._ContainerItem = container;
            this.NominalName = name;
            
            if (constantValue != null)
            {
                this.SubstantiveType = constantValue.GetType();
            }
            else {
                this.SubstantiveType = valueType;
            }
            this.Lifecycle = DependenceLifecycles.Constant;
            this._CreateInstance = () => constantValue;
            
        }

        public DependentItem Register(string name, object value, Type valueType = null)
        {
            var depItem = new DependentItem(this, name, value, valueType);
            this.NamedItems.Add(name, depItem);
            return depItem;
        }

        public DependentItem(DependentItem container, string name, Func<object> factory, DependenceLifecycles lifecycle= DependenceLifecycles.AlwaysNew) {
            this._ContainerItem = container;
            this.NominalName = name;


            this.Lifecycle = lifecycle;
            if (lifecycle == DependenceLifecycles.AlwaysNew)
            {
                this._CreateInstance = factory;
            }
            else if (lifecycle == DependenceLifecycles.Constant) {
                object constantValue = null;
                bool valueInitted = false;
                this._CreateInstance = () => {
                    if (valueInitted) return constantValue;
                    lock (this) {
                        if (valueInitted) return constantValue;
                        constantValue = factory();
                        valueInitted = true;
                    }
                    return constantValue;
                };
            }
        }

        public DependentItem Register(string name, Func<object> factory,DependenceLifecycles depType= DependenceLifecycles.AlwaysNew)
        {
            var depItem = new DependentItem(this, name, factory,depType);
            this.NamedItems.Add(name, depItem);
            return depItem;
        }



        public object CreateInstance() {
            if (this._CreateInstance == null) {
                lock (this) {
                    var gen = new CreateInstanceGenerator(this);
                    this._CreateInstance = gen.Generate();
                }
            }
            return this._CreateInstance();
        }
        

        

        internal protected Dictionary<string, DependentItem> NamedItems {
            get {
                if (this._NamedItems == null)
                {
                    this._NamedItems = new Dictionary<string, DependentItem>();
                }
                return _NamedItems;
            }
        }

        internal protected Dictionary<Guid, DependentItem> TypedItems
        {
            get
            {
                if (this._TypedItems == null)
                {
                    this._TypedItems = new Dictionary<Guid, DependentItem>();
                }
                return _TypedItems;
            }
        }

        

        public DependentItem GetDependentItem(string nominalName) {
            if (this._NamedItems == null) return null;
            DependentItem result = null;
            this._NamedItems.TryGetValue(nominalName, out result);
            return result;
        }

        public DependentItem GetDependentItem(Type nominalType)
        {
            if (this._TypedItems == null) return null;
            DependentItem result = null;
            this._TypedItems.TryGetValue(nominalType.GUID, out result);
            return result;
        }

        public DependentItem FindDepedentItem(string nominalName) {
            if (this._NamedItems == null) return null;
            DependentItem result = null;
            if (this._NamedItems.TryGetValue(nominalName, out result)) {
                return result;
            }
            if (this._ContainerItem != null) {
                return this._ContainerItem.FindDepedentItem(nominalName);
            }
            return null;
        }

        public DependentItem FindDepedentItem(Type nominalType,string nominalName=null)
        {
            if (this._TypedItems == null) return null;
            DependentItem result = null;

            if (this._TypedItems.TryGetValue(nominalType.GUID, out result))
            {
                return result;
            }
            if (nominalName != null && this._NamedItems!=null) {
                if (this._NamedItems.TryGetValue(nominalName, out result))
                {
                    if (nominalType.IsAssignableFrom(result.SubstantiveType))
                    {
                        return result;
                    }
                    else {
                        result = null;
                    }
                }
            }
            
            if (this._ContainerItem != null)
            {
                return this._ContainerItem.FindDepedentItem(nominalType,nominalName);
            }
            return null;
        }
    }
}
