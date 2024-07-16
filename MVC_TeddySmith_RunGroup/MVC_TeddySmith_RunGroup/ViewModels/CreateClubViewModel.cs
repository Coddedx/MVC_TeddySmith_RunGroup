using MVC_TeddySmith_RunGroup.Data.Enum;
using MVC_TeddySmith_RunGroup.Models;
using System.Runtime.CompilerServices;

namespace MVC_TeddySmith_RunGroup.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public string AppUserId { get; set; } //this is string cause with the ClaimsPrincipalExtensions-GetUserId is returning string  

    }
}
