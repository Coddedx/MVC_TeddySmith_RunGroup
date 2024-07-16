using Microsoft.AspNetCore.Mvc;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.ViewModels;

namespace MVC_TeddySmith_RunGroup.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)//we created the IDashboardRepository on the interfaces folder so we can pull ot from the databese so now on we dont need the ApplicationDbContext context normally we add that 
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index() //repositories are async so we have to make the whole method async (we need to return async reponse)
        {
            //ability the spesiic data returned back from database for our users
            var userRaces = await _dashboardRepository.GetAllUserRaces();   //races are spesificly for the users
            var userClubs = await _dashboardRepository.GetAllUserClubs();   //cubs are spesificly for the users

           //we are gonna create an object to hold our data inside of . thats the beuty of the view model they dont have to be within the parameters they can be anything
            var dashboardViewModel = new DashboardViewModel
            {
                Races = userRaces,
                Clubs = userClubs
            };

            return View(dashboardViewModel);
        }
    }
}
