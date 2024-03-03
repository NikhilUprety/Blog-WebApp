using TinyBlog.Models;
using X.PagedList;

namespace TinyBlog.ViewModel
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IPagedList<post>? Posts { get; set; }
    }
}
