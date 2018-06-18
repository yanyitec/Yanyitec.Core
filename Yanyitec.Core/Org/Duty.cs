using System;
using System.Collections.Generic;
using System.Text;
using Yanyitec.Repo;

namespace Yanyitec.Org
{
    public class Duty :Record
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid MemberId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid JobId { get; set; }
    }
}
