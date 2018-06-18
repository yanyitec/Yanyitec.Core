using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc
{
    public class ApiSvcControllerAttribute : Attribute
    {
        public ApiSvcControllerAttribute(string controllerName = null) {
            this.ControllerName = controllerName;
        }
        public string ControllerName { get; private set; }
    }
}
