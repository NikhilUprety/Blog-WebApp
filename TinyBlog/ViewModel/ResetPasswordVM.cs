using System.ComponentModel.DataAnnotations;

namespace TinyBlog.ViewModel
{
    public class ResetPasswordVM
    {
        public string? Id { get; set; }
        public string? Username { get; set; }

        [Required]
        public string? NewPassword{ get; set; }


        [Required]
        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword {  get; set; }
    }
}
