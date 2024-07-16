using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace MVC_TeddySmith_RunGroup.Models
{
    public class AppUser :IdentityUser //identity works only with the entity framework and the way to wire them is go to dbcontext and change the ...:DbContext to IdentityDbContext
    {
        // ? nullable yani null değer atanbilir
        //package manager console den migration yaparken(isim bulunmuyo falan derse microsft entity framework core tools indir)
        //sonrasında The entity type 'AppUser' requires a primary key to be defined. If you intended to use a keyless entity type, call
        //'HasNoKey' in 'OnModelCreating'. For more information on keyless entity types derse ıd oluştur burda
        //(app user iin ıd gerekior şimdilik çünkü entity framework u indirmediğimiz için indirdiğimizde buna gerek kalmıcak)

        //[Key]
        //public string Id { get; set; }  //buna gerek yok artık identityuser kullandığımız için 
        // NOW THE ID İS STRING (BECAUSE OF THE IDENTİTY FRAMEWORK)
        public int? Pace { get; set; }  //how much you run a weak
        public int? Mileage { get; set; }

        [ForeignKey("Address")]  //this going to link to adresses together 
        public int? AddressId { get; set; } //we want to link the addresses then go to applicationdbcontext. nullable couse when we are registering user only type email and password so this could be null. we use this only code
        public Address? Adress { get; set; }  // DİKKAT ADRESİ 1 TANE D İLE YAZMIŞIM :(((((((((((
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }
    }
}
