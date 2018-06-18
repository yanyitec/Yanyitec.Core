using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc
{
    public class ServiceEntity : Entity<string>
    {

        public string Description{get;set;}

        public string BasUrl { get; set; }
        public string HeartbeatUrl { get; set; }
        public string OnstartUrl { get; set; }
        public string OnstopUrl { get; set; }
    }
}
