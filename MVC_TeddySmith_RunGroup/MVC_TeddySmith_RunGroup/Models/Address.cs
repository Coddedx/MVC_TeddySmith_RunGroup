using System.ComponentModel.DataAnnotations;

namespace MVC_TeddySmith_RunGroup.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}
