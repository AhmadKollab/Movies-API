using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/theaters")]
    public class TheatersController : CustomBaseController
    {
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "Theaters";

        public TheatersController(AppliactionDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore):
            base(context,mapper,outputCacheStore,cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<TheatersDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Theater, TheatersDTO>(pagination, orderBy: g => g.Name);
        }


        [HttpGet("{id:int}", Name = "GetTheaterById")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TheatersDTO>> Get(int id)
        {
            return await Get<Theater, TheatersDTO>(id);
        }


        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] TheatersCreationDTO theatersCreationDTO)
        {
            return await Post<TheatersCreationDTO, Theater, TheatersDTO>(theatersCreationDTO, routeName: "GetTheaterById");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Theater>(id);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TheatersCreationDTO theatersCreationDTO)
        {
            return await Put<Theater, TheatersCreationDTO>(id, theatersCreationDTO);
        }
    }
}
