using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Utilites
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            configareGenres();
        }

        private void configareGenres() {
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<Genre, GenreDTO>();
        }
    }
}
