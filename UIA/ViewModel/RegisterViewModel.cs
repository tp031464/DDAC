using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ExpressiveAnnotations.Attributes;

namespace UIA.ViewModel
{
    public class RegisterViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required to register!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required to register!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password!")]
        [Compare("Password", ErrorMessage= "Password and confirm password doesn't match!")]
        public string ConfirmPassword { get; set; }
    }
}