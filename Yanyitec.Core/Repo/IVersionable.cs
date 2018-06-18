using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public interface IVersionable : IRecord, IVersion
    {
        /// <summary>
        /// 版本化的时间
        /// </summary>
        DateTime VersionedTime{get;set;}

        /// <summary>
        /// 版本修改者。谁打的这个版本
        /// </summary>
        Guid VersionerId { get; set; }

        /// <summary>
        /// 真实的版本修改者
        /// </summary>
        Guid VersionedBy { get; set; }

        string VersionerInfo { get; set; }

        string VersionChangeInfo { get; set; }
    }
}
