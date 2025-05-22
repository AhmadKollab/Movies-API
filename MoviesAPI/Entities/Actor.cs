using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Entities
{
    public class Actor
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirht { get; set; }
        [Unicode(false)]
        public string? Picture { get; set; }
    }
}
