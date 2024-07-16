using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.Data
{
    // Daha sonra Program a gidip builder.Services.AddDbContext<ApplicationDbContext>(options => ...
    // kodunu yaıyoruz sql connection ı sağlamak için 
    public class ApplicationDbContext : IdentityDbContext<AppUser> //DbContext is gonna inherit entity framework core. to be able to use identity framework inheritng the IdentityDbContext
    {
        //public ApplicationDbContext()
        //{

        //}

        //pass this in and pass this up to this actual this actual DbContext class that entity framework provides us 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<State> States { get; set; }
        //public DbSet<City> Cities { get; set; }

    }
}

