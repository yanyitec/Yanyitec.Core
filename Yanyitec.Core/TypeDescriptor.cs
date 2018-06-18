using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    /// <summary>
    /// C# 编译后的类型描述器，
    /// 用于动态加载类型
    /// </summary>
    public class TypeDescriptor
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string Assembly { get; set; }
    }
}
