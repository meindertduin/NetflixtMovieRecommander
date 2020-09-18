using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Forms
{
    public class WatchGroupForm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ExistingUserForm> ExistingUsers { get; set; }
        public List<string> AddedUsers { get; set; }
    }
}