using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public class RepoContext :IRepoContext
    {
        public virtual string AllowedFields { get; set; }
    }
}
