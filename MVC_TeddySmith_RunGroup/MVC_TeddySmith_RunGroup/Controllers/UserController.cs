using Microsoft.AspNetCore.Mvc;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.ViewModels;

namespace MVC_TeddySmith_RunGroup.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        
        
        [HttpGet("users")] //its better to call users order to do that even though they go to the index when they go to the actual url its gonna called users even though the controller is user 
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers(); //whenever we hit this page we want to show all the users but we dont want to show all the user so we created a view model 

            //we are going to map this without automapper its a annecesery work 
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUsersById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            return View(userDetailViewModel);
        }

    }
}
