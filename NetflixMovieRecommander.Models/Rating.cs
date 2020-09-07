using System.ComponentModel.DataAnnotations.Schema;

namespace NetflixMovieRecommander.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Value { get; set; }

        public Recommended Recommended { get; set; }
        public int RecommendedForeignKey { get; set; }
    }
}