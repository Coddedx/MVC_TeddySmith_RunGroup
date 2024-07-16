using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.Interfaces
{
    public interface IDashboardRepository 
    {
        Task<List<Race>> GetAllUserRaces();  //race yi repositery siz yapmaya çalışcam 
        Task<List<Club>> GetAllUserClubs();
        //Task<AppUser> GetUserById(string id);
        //Task<AppUser> GetByIdNoTracking(string id);
        //bool Update(AppUser user);
        //bool Save();

    }
}
