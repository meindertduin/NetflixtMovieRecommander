using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task StoreUserWatchlistItems(List<string> watchListFilePaths)
        {
            foreach(var watchlistFilePath in watchListFilePaths)
            {
                using (var reader = new StreamReader(watchlistFilePath))
                {
                    await WriteFileContentToDatabase(reader);
                }
            }
        }

        private async Task WriteFileContentToDatabase(StreamReader reader)
        {
            List<string> titles = new List<string>();
            List<DateTime> dates = new List<DateTime>();
            List<WatchItem> watchItems = new List<WatchItem>();

            string type;

            while (!reader.EndOfStream)
            {
                await ParseLineToDatabase(reader, dates, titles);
            }
        }

        private async Task ParseLineToDatabase(StreamReader reader, List<DateTime> dates, List<string> titles)
        {
            string type;
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
            
            await AddDataPartsToDatabase(dateParts, title, type);
        }

        private async Task AddDataPartsToDatabase(string[] dateParts, string title, string type)
        {
            if (dateParts.Length == 3 && title != "Title")
            {
                int days = Int32.Parse(dateParts[0]);
                int month = Int32.Parse(dateParts[1]);
                int year = Int32.Parse(String.Concat("20", dateParts[2]));

                var date = new DateTime(year, month, days);

                await _ctx.WatchItems.AddAsync(new WatchItem
                {
                    Title = title,
                    Type = type,
                    DateWatched = date,
                });

                await _ctx.SaveChangesAsync();
            }
        }
    }
}