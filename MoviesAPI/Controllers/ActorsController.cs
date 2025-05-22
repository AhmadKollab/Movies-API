using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.OutputCaching;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "actor";

        public ActorsController(AppliactionDbContext context , IMapper mapper , IOutputCacheStore outputCacheStore)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO) {
            var actor = mapper.Map<Actor>(actorCreationDTO);

            // work to do with picture

            context.Add(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag , default);
            return Ok();
        }
    }
}
