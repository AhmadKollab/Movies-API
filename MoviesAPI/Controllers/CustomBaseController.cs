using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Utilites;

namespace MoviesAPI.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;

        public CustomBaseController(AppliactionDbContext context , IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
    }
}
