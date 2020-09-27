using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FierceStukCloud.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : BaseObject
    {
        private readonly FierceStukCloudDbContextFactory _contextFactory;

        public GenericDataService(FierceStukCloudDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using(FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {
                var newEntity = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return newEntity.Entity;
            }
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
