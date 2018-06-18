using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc.Meta
{
    public class ActionMeta
    {
        public string Name { get; set; }
        public ParameterMeta Return { get; set; }

        public List<ParameterMeta> Parameters { get; set; }
    }
}
