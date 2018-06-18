using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yanyitec.Repo
{
    public interface IRepository<TID,TEntity>: IRepository
        where TEntity:class
    {
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(TID id,IRepoContext context = null);
        /// <summary>
        /// 新添实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity , IRepoContext context = null);
        /// <summary>
        /// 修改实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>
        Task<TEntity> ModifyAsync(TEntity entity, IRepoContext context = null);
        /// <summary>
        /// 根据条件，获取列表
        /// 也可用于分页
        /// </summary>
        /// <param name="pageable"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>

        Task<Pageable<TEntity>> ListAsync(Pageable<TEntity> pageable, IRepoContext context = null);
        /// <summary>
        /// 根据Id删除实体数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>
        Task<TEntity> DeleteByIdAsync(TID id, IRepoContext context = null);
        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="storagePartition"></param>
        /// <returns></returns>
        Task<TEntity> DeleteAsync(TEntity entity, IRepoContext context = null);

    }
}
