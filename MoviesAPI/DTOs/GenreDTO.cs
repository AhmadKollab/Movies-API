using MoviesAPI.Validtions;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenreDTO : IId
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
