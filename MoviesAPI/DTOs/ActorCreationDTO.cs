using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
