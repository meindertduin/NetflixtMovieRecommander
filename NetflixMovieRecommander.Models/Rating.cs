using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetflixMovieRecommander.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Source { get; set; }
        [MaxLength(20)]
        public string Value { get; set; }

        public Recommended Recommended { get; set; }
        [ForeignKey("Recommended")]
        public int RecommendedForeignKey { get; set; }
    }
}