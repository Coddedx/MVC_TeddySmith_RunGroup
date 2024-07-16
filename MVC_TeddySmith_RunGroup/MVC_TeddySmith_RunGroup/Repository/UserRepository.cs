using Microsoft.EntityFrameworkCore;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync(); //whenever you on the runners page you are seeing a list of runner so this has to be list but when you click on the spesific user its going to get the individual user by id --> we do that by the GetUsersById
        }

        public async Task<AppUser> GetUsersById(string id)
        {
            return await _context.Users.FindAsync(id);  //when you click on the spesific user its going to get the individual user by id
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false; 
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
