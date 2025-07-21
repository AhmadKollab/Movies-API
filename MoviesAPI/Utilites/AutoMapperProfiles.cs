using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Utilites
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory) {
            configareGenres();
            configareActors();
            configareTheaters(geometryFactory);
        }

        private void configareTheaters(GeometryFactory geometryFactory) {
            CreateMap<Theater, TheatersDTO>()
                .ForMember(x => x.Latitude, x => x.MapFrom(p => p.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(p => p.Location.X));
            CreateMap<TheatersCreationDTO, Theater>()
                .ForMember(entity => entity.Location, dto => dto.MapFrom(p =>
                    geometryFactory.CreatePoint(new Coordinate(p.Longitude, p.Latitude))
                    ));
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
