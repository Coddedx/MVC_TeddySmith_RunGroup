using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.Models;
using MVC_TeddySmith_RunGroup.Repository;
using MVC_TeddySmith_RunGroup.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace MVC_TeddySmith_RunGroup.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RaceController(ApplicationDbContext context, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor; 
        }

        public IActionResult Index()
        {
            var race = _context.Races.ToList();
            return View(race);
        }

        public IActionResult Detail(int id)
        {            
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(c => c.Id == id); 
            return View(race);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel
            {
                AppUserId = curUserId,
            };
            return View(createRaceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _photoService.AddPhotoAsync(raceVM.Image);
                   var race = new Race
                    {
                        Title = raceVM.Title,
                        Description = raceVM.Description,
                        Image = result.Url.ToString(),
                        AppUserId = raceVM.AppUserId,
                        Address = new Address
                        {
                            Street = raceVM.Address.Street,
                            City = raceVM.Address.City,
                            State = raceVM.Address.State,
                        }
                    };
                    _context.Add(race);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Photo upload failed");
                }
                return View(raceVM);
            }
            catch (Exception)
            {
                return View(raceVM);
            }

        }


        public async Task<IActionResult> Edit(int id)
        {

            var race = await _context.Races.FindAsync(id); 
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = (int)race.AddressId,
                Address = race.Address,
                Url = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }


        // !!!!!!!!!!!!!!!!!!!!!!!!!!  TRACKİNG HATASInı REPOSİTORYSİZ(INTERFACES(IClubRepository)) ÇÖZülMÜŞ HALİ !!!!!!!!!!!!!!!!!!!!!!!
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Faild to edit club");
                return View("Edit", raceVM);
            }
            var userRace = await _context.Races.FindAsync(id);

            if (userRace != null)
            {
                //  RESMİ SİLİYOR
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                // RESİM YÜKLÜYOR
                _context.ChangeTracker.Clear(); //sadece bunu yazarsam tracking hatası vermedi ama edit de yapmadı _context.Update(race); bundan sonra save changes diyim ki kaydosun sqle
                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);  //------------------
                _context.SaveChanges();


                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,
                };
               _context.Update(race);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(raceVM);
            }





        }

    }
}
