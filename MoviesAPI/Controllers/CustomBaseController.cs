using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Utilites;

namespace MoviesAPI.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly string cacheTag;

        public CustomBaseController(AppliactionDbContext context , IMapper mapper,IOutputCacheStore outputCacheStore, string cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.cacheTag = cacheTag;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO,
            Expression<Func<TEntity,object>> orderBy)
        where TEntity : class
        {
            var queryable = context.Set<TEntity>().AsQueryable();
            await HttpContext.InsertPahintionParameterInHeader(queryable);
            return await queryable
                .OrderBy(orderBy)
                .Paginate(paginationDTO)
                .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity,TDTO>(int id) 
            where TEntity : class
            where TDTO : IId
        {
            var entity = await context.Set<TEntity>().ProjectTo<TDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) { 
                return NotFound();
            }

            return entity;
        }


        protected async Task<CreatedAtRouteResult> Post<TCreation, TEntity, TRead>(TCreation creation , string routeName)
            where TEntity: class
            where TRead : IId
        {
            var entity = mapper.Map<TEntity>(creation);
            context.Add(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var entityDTO = mapper.Map<TRead>(entity);
            return CreatedAtRoute(routeName, new { id = entityDTO.Id }, entityDTO);
        }

        protected async Task<IActionResult> Put<TEntity, TCreation>(int id,TCreation creation)
            where TEntity : class, IId
        {
            var entityExists = await context.Set<TEntity>().AnyAsync(g => g.Id == id);

            if (!entityExists)
            {
                return NotFound();
            }

            var entity = mapper.Map<TEntity>(creation);
            entity.Id = id;

            context.Update(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        protected async Task<IActionResult> Delete<TEntity>(int id)
            where TEntity : class, IId
        {
            var deletedRecords = await context.Set<TEntity>().Where(g => g.Id == id).ExecuteDeleteAsync();

            if (deletedRecords == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();

        }


    }
}
