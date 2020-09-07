using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetflixMovieRecommander.Data;
using NetflixMovieRecommander.Models;

namespace NetflixMoviesRecommender.api.Services
{
    public class WatchListFileParserService : IWatchListFileParserService
    {
        private readonly AppDbContext _ctx;

        public WatchListFileParserService(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public void StoreUserWatchlistItems(List<string> watchListFilePaths)
        {
            foreach(var watchlistFilePath in watchListFilePaths)
            {
                using (var reader = new StreamReader(watchlistFilePath))
                {
                    List<string> titles = new List<string>();
                    List<DateTime> dates = new List<DateTime>();
                    string type;
                    
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        var title = string.Join(" ", values[0].Split('"').Where((x, i) => i % 2 != 0));

                        if (title.Split(':').Length > 0)
                        {
                            title = title.Split(':')[0];
                            type = "series";

                        }
                        else
                        {
                            type = "movie";

                        }
                        
                        var dateString = string.Join(" ", values[1].Split('"').Where((x, i) => i % 2 != 0));

                        var dateParts = dateString.Split('-');
                        if (dateParts.Length == 3 && title != "Title")
                        {
                            int days = Int32.Parse(dateParts[0]);
                            int month = Int32.Parse(dateParts[1]);
                            int year = Int32.Parse(String.Concat("20", dateParts[2]));
                        
                            var date = new DateTime(year, month, days);

                            _ctx.Add(new WatchItem
                            {
                                Title = title,
                                Type = type,
                                DateWatched = date,
                            });
                            dates.Add(date);
                            titles.Add(title);
                        }
                    }

                    _ctx.SaveChangesAsync();
                    
                    titles.RemoveAt(0);
                }
            }
        }
    }
}