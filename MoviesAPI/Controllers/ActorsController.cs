using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;
using MoviesAPI.Utilites;

namespace MoviesAPI.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : CustomBaseController
    {
        private readonly AppliactionDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IFileStorge fileStorge;
        private const string cacheTag = "actor";
        private readonly string container = "actors";

        public ActorsController(AppliactionDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IFileStorge fileStorge)
            :base(context,mapper,outputCacheStore,cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.fileStorge = fileStorge;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ActorDTO>> Get([FromQuery] PaginationDTO paginationDTO) {
            return await Get<Actor, ActorDTO>(paginationDTO, orderBy: a => a.Name);
        }

        [HttpGet("{id:int}", Name = "GetActorById")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            return await Get<Actor, ActorDTO>(id);
        }
        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromForm] ActorCreationDTO actorCreationDTO) {
            var actor = mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Picture is not null) {
                var url = await fileStorge.Store(container, actorCreationDTO.Picture);
                actor.Picture = url;
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return CreatedAtRoute("GetActorById", new { id = actor.Id }, actorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO) {
            var actor = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null) {
                return NotFound();
            }
            actor = mapper.Map(actorCreationDTO, actor);

            if (actorCreationDTO.Picture is not null) {
                actor.Picture = await fileStorge.edit(container, actor.Picture, actorCreationDTO.Picture);
            }

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            var actor = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            if (actor is null) {
                return NotFound();
            }
            
            context.Remove(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            await fileStorge.Delete(container,actor.Picture);
            return NoContent();


        }
    }
}
