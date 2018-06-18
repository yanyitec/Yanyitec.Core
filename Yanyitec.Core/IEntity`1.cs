using System;

namespace Yanyitec
{
    public interface IEntity<TID> 
    {
        TID Id { get; }

        void SetAssignedId(TID id);
        
        
    }
}