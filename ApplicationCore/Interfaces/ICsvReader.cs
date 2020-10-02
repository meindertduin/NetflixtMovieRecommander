using System;
using System.Collections.Generic;

namespace NetflixMoviesRecommender.api.Services
{
    public interface ICsvReader
    {
        public Tuple<List<string>, List<string>> ReadKeyValues(string filePath);
    }
}