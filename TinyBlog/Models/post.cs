namespace TinyBlog.Models
{
    public class post

    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ApplicationUserID { get; set; }  
        public ApplicationUser? ApplicationUser {  get; set; }  
        public DateTime CreatedTime { get; set; }= DateTime.Now;
        public string? Description { get; set; }    
        public string? slug { get; set; }

        public string? ThumbnailUrl { get; set; }


    }
}
