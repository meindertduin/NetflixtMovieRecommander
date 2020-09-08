using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetflixMovieRecommander.Models
{
    public class NetflixRecommended
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public Boolean Deleted { get; set; }
        [MaxLength(10)]
        public string Type { get; set; }
        [MaxLength(200)]
        public string Genres { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
    }
}