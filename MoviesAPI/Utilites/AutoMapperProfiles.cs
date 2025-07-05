using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Utilites
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            configareGenres();
            configareActors();
        }

        private void configareGenres() {
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<Genre, GenreDTO>();
        }

        private void configareActors()
        {
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture,options => options.Ignore());
            CreateMap<Actor, ActorDTO>();
        }
    }
}
