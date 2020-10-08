using System.Collections.Generic;

namespace NetflixMoviesRecommender.api.AppDomain.QueryResults
{
    public class WatchGroupIdsResult
    {
        public List<string> OnwGroupIds { get; set; }
        public List<string> MemberGroupsIds { get; set; }
    }
}