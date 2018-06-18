using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Repo
{
    public interface IRecord :IEntity
    {

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 名义创建者Id
        /// </summary>
        Guid CreatorId { get; set; }
        /// <summary>
        /// 实际创建者Id
        /// </summary>
        Guid CreatedBy { get; set; }
        /// <summary>
        /// 创建者信息(名义,实际)
        /// </summary>
        string CreatorInfo { get; set; }

        


        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime ModifyTime { get; set; }
        /// <summary>
        /// 名义修改者Id
        /// </summary>
        Guid ModifierId { get; set; }
        /// <summary>
        /// 实际修改者的Id
        /// </summary>
        Guid ModifiedBy { get; set; }
        /// <summary>
        /// 修改者信息(名义,实际)
        /// </summary>
        string ModifierInfo { get; set; }

        
        /// <summary>
        /// 最后修改的信息
        /// </summary>
        string LastOperationInfo { get; set; }
    }
}
