using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVC_TeddySmith_RunGroup.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }


        [Required, DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }
    }
}
