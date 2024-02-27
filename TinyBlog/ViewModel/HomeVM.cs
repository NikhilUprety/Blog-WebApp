using TinyBlog.Models;

namespace TinyBlog.ViewModel
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<post>? Posts { get; set; }
    }
}
