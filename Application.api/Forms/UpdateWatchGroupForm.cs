using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetflixMoviesRecommender.api.Forms
{
    public class UpdateWatchGroupForm
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] AddedNames { get; set; }
    }
}