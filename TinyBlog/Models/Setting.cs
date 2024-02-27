using System.ComponentModel.DataAnnotations;

namespace TinyBlog.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string?  SiteName { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? TwitterUrl {  get; set; }

        public string? LinkedinUrl {get; set; }

        public string? GithubUrl { get; set; }


    }
}
