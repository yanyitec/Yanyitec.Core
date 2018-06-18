using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public class Record : Entity, IRecord
    {
        public DateTime CreateTime { get; set; }
        public Guid CreatorId { get; set; }

        public Guid CreatedBy { get; set; }

        public string CreatorInfo { get; set; }
        public DateTime ModifyTime { get; set; }
        public Guid ModifierId { get; set; }

        public Guid ModifiedBy { get; set; }
        public string ModifierInfo { get; set; }
        public string LastOperationInfo { get; set; }
    }
}
