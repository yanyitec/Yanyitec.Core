using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Yanyitec
{
    public class Pageable<T> : ListParameters<T>
    {
        public Pageable(Expression<Func<T, bool>> initCriteria = null):base(initCriteria) {
        }

        

        public long PageCount { get; set; }

        public long PageIndex { get; set; }

        public uint PageSize { get; set; }





        static public implicit operator Pageable<T>(Expression<Func<T,bool>> expr)
        {
            return new Pageable<T>(expr);
        }


    }
}
