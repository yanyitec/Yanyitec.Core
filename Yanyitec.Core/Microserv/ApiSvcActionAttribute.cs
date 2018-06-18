using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc
{
    public class ApiSvcActionAttribute : Attribute
    {
        public ApiSvcActionAttribute(string actionName = null)
        {
            this.ActionName = actionName;
        }
        public string ActionName { get; set; }
    }
}
