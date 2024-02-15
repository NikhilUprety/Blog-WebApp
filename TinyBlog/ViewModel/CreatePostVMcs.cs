using System.ComponentModel.DataAnnotations;
using TinyBlog.Models;

namespace TinyBlog.ViewModel
{
    public class CreatePostVMcs
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ApplicationUserID { get; set; }

        public string? Description { get; set; }

        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail {  get; set; }

    }
}
