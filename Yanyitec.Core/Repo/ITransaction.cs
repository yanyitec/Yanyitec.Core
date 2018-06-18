using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public interface ITransaction :IDisposable
    {
        void Commit();
        void Rollback();

        IRepository Repository { get; }
    }
}
