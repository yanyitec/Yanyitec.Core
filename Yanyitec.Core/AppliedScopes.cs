using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    /// <summary>
    /// 作用范围
    /// 用于Role，Permission等
    /// </summary>
    public enum AppliedScopes
    {
        /// <summary>
        /// 无作用范围
        /// </summary>
        None = -1,
        /// <summary>
        /// 所有范围
        /// </summary>
        All=0,
        /// <summary>
        /// 个人
        /// </summary>
        Person,
        /// <summary>
        /// 角色
        /// </summary>
        Role,
        /// <summary>
        /// 部门
        /// </summary>
        Department,
        /// <summary>
        /// 组织
        /// </summary>
        Organization,
        /// <summary>
        /// 子系统(单独部署的微服务等)
        /// </summary>
        Subsystem,
        /// <summary>
        /// 全局
        /// </summary>
        Global
    }
}
