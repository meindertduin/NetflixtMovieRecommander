using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace NetflixMoviesRecommender.api.Domain.Extensions
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this IFormFile postedFile)
        {
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                postedFile.ContentType.ToLower() != "image/jpeg" &&
                postedFile.ContentType.ToLower() != "image/pjpeg" &&
                postedFile.ContentType.ToLower() != "image/gif" &&
                postedFile.ContentType.ToLower() != "image/x-png" &&
                postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }
        
    }
}