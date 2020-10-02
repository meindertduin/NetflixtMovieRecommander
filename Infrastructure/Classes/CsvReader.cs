using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetflixMoviesRecommender.api.Services
{
    public class CsvReader : ICsvReader
    {
        public Tuple<List<string>, List<string>> ReadKeyValues(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                List<string> keys = new List<string>();
                List<string> values = new List<string>();
                
                while (reader.EndOfStream == false)
                {
                    var line = reader.ReadLine();
                    var dataPair = line.Split(',');
                    if(!string.IsNullOrEmpty(dataPair[0]) &&  !string.IsNullOrEmpty(dataPair[1]))
                    {
                        var key = string.Join(" ", dataPair[0].Split('"').Where((x, i) => i % 2 != 0));
                        var value = string.Join(" ", dataPair[1].Split('"').Where((x, i) => i % 2 != 0));
                    
                        keys.Add(key);
                        values.Add(value);
                    }
                }
                
                return new Tuple<List<string>, List<string>>(keys, values);
            }
        }
    }
}