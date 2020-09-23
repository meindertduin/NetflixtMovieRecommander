using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetflixMovieRecommander.Models.Enums;

namespace NetflixMovieRecommander.Models
{
    public class ProfileFile
    {
        public int ProfileFileId { get; set; }
        [MaxLength(200)]
        public string FileName { get; set; }

        [MaxLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
        
        public UserProfile UserProfile { get; set; }
        public string UserProfileId { get; set; }
    }
}