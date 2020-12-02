using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FierceStukCloud.Core.Services
{
    public interface IDataService<T>
    {
        Task<T> Create(T entity);
        Task<bool> Delete(Guid guid);
        Task<T> Update(Guid guid, T entity);
        Task<T> Get(Guid guid);
        Task<IEnumerable<T>> GetAll(T entity);
    }
}
