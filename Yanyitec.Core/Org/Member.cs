using System;
using System.Collections.Generic;
using System.Text;
using Yanyitec.Repo;

namespace Yanyitec.Org
{
    public class Member:Record,IUser
    {
       

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string UserInfo { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        

        
    }
}
