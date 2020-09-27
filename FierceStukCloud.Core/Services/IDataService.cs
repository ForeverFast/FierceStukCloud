using System.Collections.Generic;
using System.Threading.Tasks;

namespace FierceStukCloud.Core.Services
{
    public interface IDataService<T>
    {
        Task<T> Create(T entity);
        Task<bool> Delete(T entity);
        Task<T> Update(T entity);
        Task<T> Get(T entity);
        Task<IEnumerable<T>> GetAll(T entity);
    }
}
