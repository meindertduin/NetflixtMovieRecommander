using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetflixMovieRecommander.Models
{
    public class Recommended
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } 
        [MaxLength(20)]
        public string Year { get; set; } 
        [MaxLength(10)]
        public string Rated { get; set; } 
        [MaxLength(30)]
        public string Released { get; set; } 
        [MaxLength(20)]
        public string Runtime { get; set; } 
        [MaxLength(100)]
        public string Genre { get; set; } 
        [MaxLength(100)]
        public string Director { get; set; } 
        [MaxLength(100)]
        public string Writer { get; set; } 
        // not specified as these lists can be quite long
        public string Actors { get; set; } 
        public string Plot { get; set; } 
        
        [MaxLength(100)]
        public string Language { get; set; }
        [MaxLength(100)]
        public string Country { get; set; } 
        [MaxLength(300)]
        public string Awards { get; set; } 
        
        // contains an url
        [MaxLength(500)]
        public string Poster { get; set; } 
        public List<Rating> Ratings { get; set; } 
        [MaxLength(20)]
        public string Metascore { get; set; } 
        [MaxLength(10)]
        public string imdbRating { get; set; } 
        [MaxLength(10)]
        public string imdbVotes { get; set; } 
        [MaxLength(20)]
        public string imdbID { get; set; } 
        [MaxLength(20)]
        public string Type { get; set; } 
        [MaxLength(10)]
        public string totalSeasons { get; set; }
        public Boolean Response { get; set; }
    }
}