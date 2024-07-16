using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();

        Task <AppUser> GetUsersById(string id); // the app user is a actually a string 

        bool Add(AppUser user);
        bool Update(AppUser user);  
        bool Delete(AppUser user);
        bool Save();
        
    }
}
