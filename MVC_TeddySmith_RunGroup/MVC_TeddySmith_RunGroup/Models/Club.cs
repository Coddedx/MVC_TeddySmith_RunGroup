using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MVC_TeddySmith_RunGroup.Data.Enum;

namespace MVC_TeddySmith_RunGroup.Models
{
    public class Club
    {
        //primary key is parent(only one person can be a parent), foreignKey is child.
        //Only one primarykey can exist but multiple foreignkey could be exist
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
       
        [ForeignKey("Address")]
        public int? AddressId { get; set; }  // bu olmadan yani foreign key olmadan da olur ama sonrasında kontrol etmek daha zor olur   
        public Address? Address { get; set; }  //AddressId ile one to many ilişkisi var (adress classı ile)
      
        public ClubCategory ClubCategory { get; set; }
    
        
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }  //identity framework kullandığımız için appuser ıd si string 
        public AppUser? AppUser { get; set; }
    }
}
