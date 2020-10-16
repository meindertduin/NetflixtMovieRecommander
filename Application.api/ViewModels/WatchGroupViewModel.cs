using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetflixMovieRecommander.Models
{
    public class WatchGroupViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserProfileViewModel Owner { get; set; }
        public List<UserProfileViewModel> Members { get; set; }
        public string[] AddedNames { get; set; }
    }
}