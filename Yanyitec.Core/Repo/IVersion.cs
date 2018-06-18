using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public interface IVersion : IEntity
    {
        Guid VersionId { get; set; }
    }
}
