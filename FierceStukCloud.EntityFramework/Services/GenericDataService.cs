using FierceStukCloud.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using (FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public async Task<bool> Delete(Guid guid)
        {
            using (FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);
                context.Set<T>().Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<T> Get(Guid guid)
        {
            using (FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAll(T entity)
        {
            using (FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {

                IEnumerable<T> entities;
                switch (entity.ToString())
                {
                    case "FierceStukCloud.Core.Song":

                        //entities = await context.Songs.Include(x => x.DbAlbums).ThenInclude(x => x.Album)
                        //      .Include(x => x.DbAuthors).ThenInclude(x => x.Author)
                        //      .Include(x => x.DbPlayLists).ThenInclude(x => x.PlayList)
                        //      .ToListAsync();

                        break;

                    case "FierceStukCloud.Core.Album":

                        break;
                    case "FierceStukCloud.Core.PlayList":

                        break;
                    case "FierceStukCloud.Core.LocalFolder":

                        break;
                }

                //IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                //return entities;
                return null;
            }
        }

        public async Task<T> Update(Guid guid, T entity)
        {
            using (FierceStukCloudDbContext context = _contextFactory.CreateDbContext(null))
            {
                entity.Id = guid;

                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }
    }
}
