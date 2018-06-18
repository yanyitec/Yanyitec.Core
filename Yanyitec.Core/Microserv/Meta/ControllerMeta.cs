using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Svc.Meta
{
    public class ControllerMeta
    {
        public string Name { get; set; }
        public List<ActionMeta> Actions { get; set; }
    }
}
