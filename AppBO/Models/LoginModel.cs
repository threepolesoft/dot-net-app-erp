using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(30, ErrorMessage = "User Name length can't be more than 30 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[StringLength(30, MinimumLength =6, ErrorMessage = "Password length minimum 6 characters.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;
    }
}
