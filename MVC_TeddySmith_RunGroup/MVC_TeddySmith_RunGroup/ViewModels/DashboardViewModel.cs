using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.ViewModels
{
    public class DashboardViewModel
    {
        //we are going to returns a list so it should be list
        public List<Race> Races { get; set; }
        public List<Club> Clubs { get; set; }
    }
}
