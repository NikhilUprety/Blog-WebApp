using System.ComponentModel.DataAnnotations;

namespace TinyBlog.ViewModel
{
    public class PageVMcs
    {
        public int Id { get; set; }

        [Required]
        public string? Tittle { get; set; }
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
