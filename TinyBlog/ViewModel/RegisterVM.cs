﻿using System.ComponentModel.DataAnnotations;

namespace TinyBlog.ViewModel
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string? UserName { get; set; }




    }
}
