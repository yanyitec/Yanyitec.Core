using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public class Entity<TID> : IEntity<TID>
    {
        public TID Id { get; protected set; }

        public void SetAssignedId(TID id) { this.Id = id; }
        
        
        
    }
}
