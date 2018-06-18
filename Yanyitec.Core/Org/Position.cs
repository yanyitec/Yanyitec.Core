using System;
using System.Collections.Generic;
using System.Text;
using Yanyitec.Repo;

namespace Yanyitec.Org
{
    /// <summary>
    /// 等级
    /// </summary>
    public class Position :Record
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Level { get; set; }
        public string LevelCode { get; set;}

        public Guid? DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
