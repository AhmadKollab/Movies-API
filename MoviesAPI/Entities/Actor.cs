using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;

namespace MoviesAPI.Entities
{
    public class Actor : IId
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Unicode(false)]
        public string? Picture { get; set; }
    }
}
