using System.ComponentModel.DataAnnotations;
using MoviesAPI.Validtions;

namespace MoviesAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "the {0} field is required")]
        [StringLength(maximumLength:18)]
        [FirstLatterUpperCase]
        public required string Name { get; set; }
        
    }
}
