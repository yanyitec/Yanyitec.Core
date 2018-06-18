using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public interface IRequest
    {
        HttpMethods Method { get; }
        IStringDictionary Arguments { get;  }
        IStringDictionary Datas { get;  }

        IStringDictionary Cookies { get; }

        IStringDictionary Headers { get; }

        string this[string key] { get; set; }

        JToken Json { get; set; }
    }
}
