using System;
using System.Collections.Generic;
using System.Text;
using Yanyitec.Repo;

namespace Yanyitec.Org
{
    public class Department : Record
    {

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 部门图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 部门路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 部门领导
        /// </summary>
        public Guid LeaderId { get; set; }
        /// <summary>
        /// 部门领导信息
        /// </summary>
        public string LeaderInfo { get; set; }

        /// <summary>
        /// 上级部门Id,没有上级部门就是一个org
        /// </summary>
        public Guid? SuperDepartmentId { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        public Department SuperDepartment { get; set; }
        /// <summary>
        /// 下级部门
        /// </summary>
        public IList<Department> SubDepartments { get; set; }


    }
}
