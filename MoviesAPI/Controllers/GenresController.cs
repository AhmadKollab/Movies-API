using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Utilites;

namespace MoviesAPI.Controllers
{

    [ApiController]
    [Route("api/genres")]
    public class GenresController : CustomBaseController
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "genre";

        public GenresController(IOutputCacheStore outputCacheStore, AppliactionDbContext context, IMapper mapper)
            :base(context,mapper,outputCacheStore,cacheTag)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GenreDTO>> Get([FromQuery] PaginationDTO pagination) {
            return await Get<Genre, GenreDTO>(pagination, orderBy: g => g.Name);
        }


        [HttpGet("{id:int}", Name = "GetGenreById")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            return await Get<Genre, GenreDTO>(id);
        }


        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            return await Post<GenreCreationDTO, Genre, GenreDTO>(genreCreationDTO, routeName: "GetGenreById");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Genre>(id);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id,[FromBody]GenreCreationDTO genreCreationDTO) 
        {
            return await Put<Genre, GenreCreationDTO>(id, genreCreationDTO);
        }
    }
}