using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Sockets;
using System.Reflection.Metadata;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.ViewModels;
using CloudinaryDotNet.Actions;

namespace MVC_TeddySmith_RunGroup.Controllers
{
    public class ClubController : Controller
    {
        //private readonly ApplicationDbContext _clubRepository;  //----------------- önceden bunu kullanıyoduk ama repository falan gelince alttakini kullanmaya başladık
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        //bunu yazdıktan sonra inject ediyoruz (context üsütne geldikten sonra kenardaki süpürge gibi olan şeye tıklıyoruz create field yazana tıklıyoruz
        //private readonly ApplicationDbContext context; kodu çıkıyor 

        private readonly IHttpContextAccessor _httpContextAccessor;    //adding the ability for the server to return the app user id 



        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)  //ApplicationDbContext this our data base. this controlling our access to our date base (ama IClubRepository clubRepository ile değiştirdik sonradan)
        {
            _clubRepository = clubRepository;  //---------- önceden _context = context idi
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        //controller are build our url !!!!!!!!!!1
        //controller control where is entering but doesnt control this view is being seen (return view diyor ya) o yüzden
        //views folderına gidip club view ı oluşturmamız da gerekiyor (view folderında Index olan burdaki index le aynı isimde olması için değiştirmedik)

        // An ActionResult is a return type of a controller method, also called an action method, and serves as the base class for *Result classes.
        // Action methods return models to views, file streams, redirect to other controllers, or whatever is necessary for the task at hand.
        //It specifies how a response is going to be given for a Specific Request.
        public async Task<IActionResult> Index() // ------------ önceden IActionResult Index() di sadece
        {
            //so you coming in and it give you the model (var clubs...) and its returning the view ( return View(clubs) )
            //_clubRepository.Clubs -> this is bringing our whole databese to program. ToList -> we need to call this to actualy execute the sql so this gonna build whose query its gonna send it to the database
            IEnumerable<Club> clubs = await _clubRepository.GetAll();  // ----------------------- önceden var clubs = _clubRepository.ToList() idi           

            //List<Club> yazabiliriz var yerine
            //(ama clubs listesinin içi boşsa null hatası alır o yüzden var daha sağlıklı)
            return View(clubs);  //your person is coming in and hitting this controller and this is going to be whats returned to the viewer 
            //(return viewer yerine başka şeyler de döndürülebilir uygulamanın isteklerine göre)

            //now we drop our model off to view and now we need to go back to the actual view(view-club-ındex) so our view actually see our data -> @model IEnumerable<Club>
        }


        public IActionResult Detail(int id) 
        {
            //join table yaptığımızda hep bir tablodan diğerine geçiş yap için include kullanmalıyız yoksa kod hata veriyor view club details da
            //include belirtilen değerin dizi öğelerinde olup olmadığını test eder. Metod, belirtilen değerin içinde geçmesini değil, tam olarak eşleşip eşleşmediğini kontrol eder.
            //clubs dediğimiz table (yani onun ismi dikkat)
            
            var club = _clubRepository.GetByIdAsync(id);  ////----------------------------- önceden
                                                          //var... = ...Clubs.Include(a => a.Address).FirstOrDefault(c => c.Id==id) idi
            return View(club);
        }


        [HttpGet]
        public IActionResult Create()
        {
            //we need to get the user id so we need the get that form the server 
            //before it shows the web page to the user it going to pull http context and its going to get that user of the http context 
            // BUT WHEN YOU PULL THE HTTP CONTEXT you get a huge object that hard to manage so its just better to abstract this away so you can just quickly grab the value that you want you dont have to worry about digging through this massive object ---> create a class called ClaimsPrincipalExtensions
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId(); //GetUserId() this comes from ClaimsPrincipalExtensions -> with that its going to get our actually id and we dont have to go to the database (much more easier). itsgonna get string so on the view model 


            //we are gonna use our view model and we are going to pass our currently user id 
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId,
            };

            
            return View(createClubViewModel); 
        }


        //uploading photo: we are not going to directly upload our database.the is a lot of limitations on that.espaiclly social media we are going to have to outsource our photo to photo bucket, bucket, blob database ext...essentially we're going to outsource this anothor piece of 100 free software like Azure, Cloudinary. Cloudinary is good alternative for small projects or when you're building mvp with azure is a little bit more complicated
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  return View(club);  //resim eklemeden önce sadece bunu diyorduk ama eklemek istediğimiz için değişiklikler yaptık. normalde metod (Club club) değeri alıyodu sadece

                    //there is different types of ways that you can control passing state in our vies but whenever your views more and more complicated all of the times it just makes sense to create view models for our views (we add ViewModels folder and on this we created a class called CreateClubViewModel)
                    var result = await _photoService.AddPhotoAsync(clubVM.Image);

                    //we could use outo mapper for this but auto mapper is going to way overkill in this situation 
                    //yani burda kullanıcın siteden girdiği modeli kaydettiğimiz CreateClubViewModelden alıp veri tabanımız olan asıl veri tabanımıza kaydediyoruz
                    var club = new Club //Club tablomuzdan club isimliyeni bir obje oluşturuyoruz ki veri tabınımıza çektiklerimizi kaydedelim
                    {
                        Title = clubVM.Title,
                        Description = clubVM.Description,
                        Image = result.Url.ToString(), //we are going to return the result that we get back from cloudinary 
                        AppUserId = clubVM.AppUserId, //kullanıcılara göre club,race falan göstermeden önvce bu yoktu ama artık kullanıcılar da club create edeb için hangi kullanıcı bunu yapmaya çalışıyo bilmemiz lazım o yüzden get creati ile kullaıcı id sini almıştık şimdi onu model e gönderiyoruz
                        Address = new Address
                        {
                            Street = clubVM.Address.Street,
                            City = clubVM.Address.City,
                            State = clubVM.Address.State,
                        }
                    };
                     _clubRepository.Add(club);
                    _clubRepository.Save(); //veri tabanımıza kaydedioruz ------------------önceden savechanges di
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Photo upload failed");
                }
                return View(clubVM);
            }
            catch (Exception)
            {
                return View(clubVM);
            }
        }


        public async Task<IActionResult> Edit (int id) //create den farkı id alması 
        {
           
            var club = await _clubRepository.GetByIdAsync(id);  //----------------------------- önceden .Clubs.FindAsync idi
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = (int)club.AddressId,
                Address = club.Address,
                Url = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Faild to edit club");
                return View("Edit",clubVM);
            }
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);   //----------------------------- önceden .Clubs.FindAsync idi
           
            if (userClub !=null)
            { 
                //  RESMİ SİLİYOR
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image); //userclub boş değilse önceki fotoğrafı silicek
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                // RESİM YÜKLÜYOR
                 var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                 var club = new Club
                 {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(), //whenever cloudinay actuallly returns to us is whats we are gonna pass into there 
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                 };


                //if you tracking two database context in the same controller or the same context you get an tarcking error to solve this go to clubrepository and create GetByIdAsyncNoTracking method and update ınterfaces(IClubRepository) add GetByIdAsyncNoTracking task and after that use the GetByIdAsyncNoTracking method instead of GetByIdAsync
                _clubRepository.Update(club);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(clubVM);
            }
        }


        // !!!!!!!!!!!!!!!!!!!! BURAYI KOPYALA YAPIŞTIR YAPTIM SONRA TKERAR BAK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")] //get delete si ile aynı değişkenleri ald için action name ile tekrar adlandırmalıyız
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);

            if (clubDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(clubDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(clubDetails.Image);
            }
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }




    }
}
