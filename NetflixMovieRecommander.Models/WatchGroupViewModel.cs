using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetflixMovieRecommander.Models
{
    public static class WatchGroupViewModel
    {
        public static readonly Func<WatchGroup, object> Create = Projection.Compile();

        public static Expression<Func<WatchGroup, object>> Projection =>
            group => new
            {
                group.Id,
                group.Title,
                group.Description,
                group.Owner,
                group.Members,
                group.AddedNames
            };
    }
}