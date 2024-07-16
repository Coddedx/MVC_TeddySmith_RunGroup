using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) //we have to ıdentify the user spesificly we have to use IHttpContextAccessor-> this is a giant object where you can accsess functionality direcly from the web page 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            //HttpContext encapsulates all information about an individual HTTP request and response. An HttpContext instance is initialized when an HTTP request is received. The HttpContext instance is accessible by middleware and app frameworks such as Web API controllers, Razor Pages, SignalR, gRPC, and more.
            //Bir istek geldiği zaman, uygulamamız hangi aksiyonları yürütmüş, istek hangi aşamalardan geçmiş sorusunun cevabına sık sık ihtiyaç duyarız. Bu soruların cevapları üzerinden, uygulamamızı daha efektif hale getirebilir veya oluşmuş hataların sebeplerini bulup bunları düzeltebiliriz.
            //curUser--> current user .we are gonna make this null or else it will give us an error
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId(); 
            var userClubs =  _context.Clubs.Where(r => r.AppUser.Id == curUser); //normalde curUser.ToString() idi ama ClaimsPrincipalExtensions-GetUserId yaptığımız                                                                       için daha düzenli durması adına onu kullanıcaz (string döndürüyodu o)
            return userClubs.ToList();
        }
        
        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId(); 
            var userRaces =  _context.Races.Where(r => r.AppUser.Id == curUser);
            return userRaces.ToList();
        }

    }
}
