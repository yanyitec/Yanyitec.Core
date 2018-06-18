using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc.Meta
{
    public class ServiceMeta
    {
        public string ServiceId { get; set; }
        public List<ControllerMeta> Controllers { get; set; }
    }
}
