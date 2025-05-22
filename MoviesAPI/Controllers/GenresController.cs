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
    public class GenresController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "genre";

        public GenresController(IOutputCacheStore outputCacheStore, AppliactionDbContext context, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GenreDTO>> Get([FromQuery] PaginationDTO pagination) {
            var queryable = context.Genres;
            await HttpContext.InsertPahintionParameterInHeader(queryable);
            return await queryable
                .OrderBy(g => g.Name)
                .Paginate(pagination)
                .ProjectTo<GenreDTO>(mapper.ConfigurationProvider).ToListAsync();
        }


        [HttpGet("{id:int}", Name = "GetGenreById")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var genre = await context.Genres.ProjectTo<GenreDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null) {
                return NotFound();
            }
            return genre;
        }


        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = mapper.Map<Genre>(genreCreationDTO);
            context.Add(genre);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var genreDTO = mapper.Map<GenreDTO>(genre);
            return CreatedAtRoute("GetGenreById", new { id = genreDTO.Id }, genreDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedRecords = await context.Genres.Where(g => g.Id == id).ExecuteDeleteAsync();

            if (deletedRecords == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id,[FromBody]GenreCreationDTO genreCreationDTO) 
        { 
            var genreExists = await context.Genres.AnyAsync(g => g.Id == id);

            if (!genreExists) {
                return NotFound();
            }

            var genre = mapper.Map<Genre>(genreCreationDTO);
            genre.Id = id;

            context.Update(genre);
            await context.SaveChangesAsync();
            
            return NoContent();


        }
    }
}