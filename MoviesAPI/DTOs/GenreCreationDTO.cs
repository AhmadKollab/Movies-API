using MoviesAPI.Validtions;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenreCreationDTO
    {
        [Required(ErrorMessage = "the {0} field is required")]
        [StringLength(maximumLength: 18)]
        [FirstLatterUpperCase]
        public required string Name { get; set; }
    }
}
