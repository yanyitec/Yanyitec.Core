using System;
using System.Collections.Generic;
using System.Text;
using Yanyitec.Repo;

namespace Yanyitec.Org
{
    /// <summary>
    /// 工作
    /// </summary>
    public class Job : Record
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }

        public Guid? PositionId { get; set; }
    }
}
