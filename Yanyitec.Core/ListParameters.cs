using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Yanyitec
{
    public class ListParameters<T> : Criteria<T>, IEnumerable<T>
    {
        public object Parameters { get; set; }

        public ListParameters(Expression<Func<T, bool>> initCriteria = null) : base(initCriteria) {
        }
        [Newtonsoft.Json.JsonIgnore]
        public long RecordCount { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Expression<Func<T, object>> Asc { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Expression<Func<T, object>> Desc { get; set; }

        IList<T> _Items;

        public IList<T> Items
        {
            get
            {
                if (this.RecordCount == 0) return null;
                if (_Items == null) _Items = new List<T>();
                return _Items;
            }
            set
            {
                this._Items = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items == null ? null : Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items == null ? null : Items.GetEnumerator();
        }
    }
}
