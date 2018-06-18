using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec
{
    public interface IUser
    {
        /// <summary>
        /// 用户唯一全局Id
        /// </summary>
        Guid UserId { get;}
        

        /// <summary>
        /// 用户唯一名
        /// </summary>
        string UserName { get;  }
        /// <summary>
        /// 用于显示的名称
        /// </summary>
        string DisplayName { get; }

        string UserInfo { get; }

        
    }
}
