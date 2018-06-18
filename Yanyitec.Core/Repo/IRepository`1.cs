using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yanyitec.Repo
{
    public interface IRepository<T>: IRepository<Guid,T>
        where T:class
    {
        
    }
}
